using System;
using System.Windows.Forms;
using Microsoft.Win32;
using CipherUnlockProV1.Licensing;

namespace CipherUnlockProV1
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal de la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Inicializar sistema de licenciamiento
                LicenseManager.Instance.Initialize();

                // Verificar si el usuario tiene una sesión válida almacenada
                var sessionInfo = SessionManager.GetStoredSession();

                if (sessionInfo != null && SessionManager.ValidateSession(sessionInfo))
                {
                    // Validar sesión con el servidor
                    var isValidRemote = ValidateRemoteSession(sessionInfo);

                    if (isValidRemote)
                    {
                        // Obtener información de licencia actualizada
                        var licenseInfo = GetUpdatedLicenseInfo(sessionInfo);
                        if (licenseInfo != null && licenseInfo.IsValid)
                        {
                            // Sesión y licencia válidas, ir directamente al formulario principal
                            LicenseManager.Instance.SetCurrentLicense(licenseInfo);
                            Application.Run(new Form1(licenseInfo));
                            return;
                        }
                        else
                        {
                            // Licencia expirada o inválida
                            SessionManager.ClearSession();
                            ShowMessage("Tu licencia ha expirado. Por favor, inicia sesión nuevamente.",
                                "Licencia Expirada", MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        // Sesión inválida en el servidor
                        SessionManager.ClearSession();
                    }
                }

                // No hay sesión válida o licencia válida, mostrar formulario de inicio
                ShowStartupForm();
            }
            catch (Exception ex)
            {
                ShowMessage($"Error crítico al iniciar la aplicación:\n\n{ex.Message}\n\nDetalles: {ex}",
                    "Error Crítico", MessageBoxIcon.Error);

                // Intentar limpiar cualquier sesión corrupta
                try
                {
                    SessionManager.ClearSession();
                }
                catch { }
            }
        }

        private static bool ValidateRemoteSession(SessionManager.SessionInfo sessionInfo)
        {
            try
            {
                // Crear un task para la validación asíncrona y convertirlo a síncrono
                var task = RemoteLicenseManager.Instance.ValidateSessionAsync(sessionInfo.Username, sessionInfo.SessionToken);
                var result = task.GetAwaiter().GetResult();

                return result.Success && result.Data;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error validando sesión remota: {ex.Message}");
                return false;
            }
        }

        private static LicenseInfo GetUpdatedLicenseInfo(SessionManager.SessionInfo sessionInfo)
        {
            try
            {
                // Obtener información actualizada de la licencia
                var task = RemoteLicenseManager.Instance.GetLicenseInfoAsync(sessionInfo.Username, sessionInfo.SessionToken);
                var result = task.GetAwaiter().GetResult();

                if (result.Success && result.Data != null)
                {
                    return result.Data;
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo información de licencia: {ex.Message}");
                return null;
            }
        }

        private static void ShowStartupForm()
        {
            using (var startupForm = new StartupForm())
            {
                var result = startupForm.ShowDialog();
                if (result == DialogResult.OK && startupForm.LicenseInfo != null)
                {
                    // Usuario autenticado exitosamente con licencia válida
                    LicenseManager.Instance.SetCurrentLicense(startupForm.LicenseInfo);
                    Application.Run(new Form1(startupForm.LicenseInfo));
                }
                else
                {
                    // Usuario canceló o no pudo autenticarse
                    Application.Exit();
                }
            }
        }

        private static void ShowMessage(string message, string title, MessageBoxIcon icon)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }
    }

    /// <summary>
    /// Gestor de sesiones usando el Registro de Windows
    /// </summary>
    public static class SessionManager
    {
        private const string REGISTRY_KEY = @"SOFTWARE\CipherUnlockProV1";
        private const string SESSION_SUBKEY = "Session";

        public class SessionInfo
        {
            public string Username { get; set; }
            public string SessionToken { get; set; }
            public DateTime ExpiryDate { get; set; }
            public string LicenseKey { get; set; }

            public bool IsExpired => DateTime.Now > ExpiryDate;
        }

        /// <summary>
        /// Almacenar información de sesión en el registro
        /// </summary>
        public static void StoreSession(SessionInfo sessionInfo)
        {
            try
            {
                using (var key = Registry.CurrentUser.CreateSubKey($"{REGISTRY_KEY}\\{SESSION_SUBKEY}"))
                {
                    if (key != null)
                    {
                        key.SetValue("Username", sessionInfo.Username ?? "", RegistryValueKind.String);
                        key.SetValue("SessionToken", sessionInfo.SessionToken ?? "", RegistryValueKind.String);
                        key.SetValue("ExpiryDate", sessionInfo.ExpiryDate.ToBinary(), RegistryValueKind.QWord);
                        key.SetValue("LicenseKey", sessionInfo.LicenseKey ?? "", RegistryValueKind.String);
                        key.SetValue("StoredDate", DateTime.Now.ToBinary(), RegistryValueKind.QWord);
                    }
                }

                System.Diagnostics.Debug.WriteLine($"Sesión almacenada para usuario: {sessionInfo.Username}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error almacenando sesión: {ex.Message}");
            }
        }

        /// <summary>
        /// Recuperar información de sesión del registro
        /// </summary>
        public static SessionInfo GetStoredSession()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey($"{REGISTRY_KEY}\\{SESSION_SUBKEY}"))
                {
                    if (key == null) return null;

                    var username = key.GetValue("Username")?.ToString();
                    var sessionToken = key.GetValue("SessionToken")?.ToString();
                    var expiryBinary = key.GetValue("ExpiryDate");
                    var licenseKey = key.GetValue("LicenseKey")?.ToString();

                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(sessionToken) || expiryBinary == null)
                        return null;

                    var expiryDate = DateTime.FromBinary((long)expiryBinary);

                    var sessionInfo = new SessionInfo
                    {
                        Username = username,
                        SessionToken = sessionToken,
                        ExpiryDate = expiryDate,
                        LicenseKey = licenseKey
                    };

                    System.Diagnostics.Debug.WriteLine($"Sesión recuperada para usuario: {username}");
                    return sessionInfo;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error recuperando sesión: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Validar si la sesión almacenada es válida
        /// </summary>
        public static bool ValidateSession(SessionInfo sessionInfo)
        {
            if (sessionInfo == null) return false;

            // Verificar si la sesión ha expirado
            if (sessionInfo.IsExpired)
            {
                System.Diagnostics.Debug.WriteLine("Sesión expirada localmente");
                return false;
            }

            // Verificar que tenga los campos requeridos
            if (string.IsNullOrEmpty(sessionInfo.Username) ||
                string.IsNullOrEmpty(sessionInfo.SessionToken))
            {
                System.Diagnostics.Debug.WriteLine("Sesión incompleta");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Limpiar sesión almacenada
        /// </summary>
        public static void ClearSession()
        {
            try
            {
                Registry.CurrentUser.DeleteSubKeyTree($"{REGISTRY_KEY}\\{SESSION_SUBKEY}", false);
                System.Diagnostics.Debug.WriteLine("Sesión limpiada correctamente");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error limpiando sesión: {ex.Message}");
            }
        }

        /// <summary>
        /// Verificar si existe una sesión almacenada
        /// </summary>
        public static bool HasStoredSession()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey($"{REGISTRY_KEY}\\{SESSION_SUBKEY}"))
                {
                    return key != null &&
                           !string.IsNullOrEmpty(key.GetValue("Username")?.ToString()) &&
                           !string.IsNullOrEmpty(key.GetValue("SessionToken")?.ToString());
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Actualizar token de sesión manteniendo otros datos
        /// </summary>
        public static void UpdateSessionToken(string newToken, DateTime newExpiry)
        {
            var existingSession = GetStoredSession();
            if (existingSession != null)
            {
                existingSession.SessionToken = newToken;
                existingSession.ExpiryDate = newExpiry;
                StoreSession(existingSession);
            }
        }

        /// <summary>
        /// Actualizar clave de licencia en la sesión existente
        /// </summary>
        public static void UpdateLicenseKey(string licenseKey)
        {
            var existingSession = GetStoredSession();
            if (existingSession != null)
            {
                existingSession.LicenseKey = licenseKey;
                StoreSession(existingSession);
            }
        }
    }
}