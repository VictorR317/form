using CipherUnlockProV1.Licensing;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using LicenseManager = CipherUnlockProV1.Licensing.LicenseManager;

namespace CipherUnlockProV1
{
    public partial class StartupForm : Form
    {
        public LicenseInfo LicenseInfo { get; private set; }
        private bool _isRegistering = false;
        private void SetupPlaceholders()
        {
            txtLicenseKey.ForeColor = Color.Gray;
            txtLicenseKey.Text = "Ingresa tu clave de licencia aquí";

            txtLicenseKey.Enter += (s, e) => {
                if (txtLicenseKey.Text == "Ingresa tu clave de licencia aquí")
                {
                    txtLicenseKey.Text = "";
                    txtLicenseKey.ForeColor = Color.Black;
                }
            };

            txtLicenseKey.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtLicenseKey.Text))
                {
                    txtLicenseKey.Text = "Ingresa tu clave de licencia aquí";
                    txtLicenseKey.ForeColor = Color.Gray;
                }
            };
        }

        public StartupForm()

        {
            InitializeComponent();
            SetupFormAppearance();
        }

        private void SetupFormAppearance()
        {
            this.Text = "CipherUnlock Pro V1 - Authentication";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(450, 550);
            this.BackColor = Color.FromArgb(40, 40, 40);

            // Set form icon if available
            try
            {
                // Intentar cargar ícono si existe
                this.Icon = new System.Drawing.Icon("icon.ico");
            }
            catch
            {
                // Si no hay ícono, continuar sin él
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (_isRegistering)
            {
                RegisterUser();
            }
            else
            {
                LoginUser();
            }
        }

        private async void LoginUser()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowMessage("Please enter both username and password.", "Validation Error", MessageBoxIcon.Warning);
                return;
            }

            SetControlsEnabled(false);
            lblStatus.Text = "Logging in...";
            lblStatus.ForeColor = Color.Yellow;

            try
            {
                var response = await RemoteLicenseManager.Instance.LoginAsync(txtUsername.Text.Trim(), txtPassword.Text);

                if (response.Success && response.Data != null)
                {
                    // Store session information
                    var sessionInfo = new SessionManager.SessionInfo
                    {
                        Username = response.Data.Username,
                        SessionToken = response.Data.SessionToken,
                        ExpiryDate = response.Data.SessionExpiry,
                        LicenseKey = response.Data.License?.LicenseKey
                    };

                    SessionManager.StoreSession(sessionInfo);
                    LicenseManager.Instance.SetCurrentLicense();

                    if (response.Data.License != null && response.Data.License.IsValid)
                    {
                        LicenseInfo = response.Data.License;
                        lblStatus.Text = "Login successful! License is active.";
                        lblStatus.ForeColor = Color.Green;

                        await Task.Delay(1000); // Brief pause to show success message
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        // User logged in but no valid license
                        ShowLicenseActivationPanel(sessionInfo);
                    }
                }
                else
                {
                    ShowMessage(response.Error ?? "Login failed. Please check your credentials.", "Login Failed", MessageBoxIcon.Error);
                    lblStatus.Text = "Login failed.";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"An error occurred during login: {ex.Message}", "Error", MessageBoxIcon.Error);
                lblStatus.Text = "Login error.";
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }


        private async void RegisterUser()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                ShowMessage("Por favor completa todos los campos.", "Error de Validación", MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                ShowMessage("Las contraseñas no coinciden.", "Error de Validación", MessageBoxIcon.Warning);
                return;
            }

            SetControlsEnabled(false);
            lblStatus.Text = "Registrando usuario...";
            lblStatus.ForeColor = Color.Yellow;

            try
            {
                var userInfo = new UserInfo
                {
                    Username = txtUsername.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Password = txtPassword.Text
                };

                // DEBUG: Mostrar datos que se envían
                var debugInfo = $"Enviando datos:\nUsuario: {userInfo.Username}\nEmail: {userInfo.Email}\nContraseña: {new string('*', userInfo.Password.Length)}";
                System.Diagnostics.Debug.WriteLine(debugInfo);

                var response = await RemoteLicenseManager.Instance.RegisterUserAsync(userInfo);

                // DEBUG: Mostrar respuesta completa
                var responseDebug = $"Respuesta del servidor:\nSuccess: {response.Success}\nMessage: {response.Message}\nError: {response.Error}";
                System.Diagnostics.Debug.WriteLine(responseDebug);

                if (response.Success)
                {
                    ShowMessage("¡Registro exitoso! Ahora puedes iniciar sesión con tus credenciales.", "Registro Exitoso", MessageBoxIcon.Information);
                    SwitchToLoginMode();
                    lblStatus.Text = "Registro exitoso. Por favor inicia sesión.";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    // Mostrar error detallado
                    string errorMessage = !string.IsNullOrEmpty(response.Error) ? response.Error : "Error desconocido durante el registro";
                    ShowMessage($"Error durante el registro:\n\n{errorMessage}", "Error de Registro", MessageBoxIcon.Error);
                    lblStatus.Text = "Error en el registro.";
                    lblStatus.ForeColor = Color.Red;

                    // DEBUG: Log completo en consola de debug
                    System.Diagnostics.Debug.WriteLine($"ERROR DETALLADO: {errorMessage}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                var networkError = $"Error de conexión de red:\n{httpEx.Message}\n\nVerifica:\n- Conexión a internet\n- URL de la API: https://www.cipherunlock.xyz/api\n- Certificado SSL del servidor";
                ShowMessage(networkError, "Error de Conexión", MessageBoxIcon.Error);
                lblStatus.Text = "Error de conexión.";
                lblStatus.ForeColor = Color.Red;
                System.Diagnostics.Debug.WriteLine($"NETWORK ERROR: {httpEx}");
            }
            catch (Exception ex)
            {
                var generalError = $"Error inesperado durante el registro:\n{ex.Message}";
                ShowMessage(generalError, "Error", MessageBoxIcon.Error);
                lblStatus.Text = "Error inesperado.";
                lblStatus.ForeColor = Color.Red;
                System.Diagnostics.Debug.WriteLine($"GENERAL ERROR: {ex}");
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        private void ShowLicenseActivationPanel(SessionManager.SessionInfo sessionInfo)
        {
            panelLicense.Visible = true;
            lblLicenseInfo.Text = "No active license found. Please activate a license key or purchase a new license.";

            // Store session info for license operations
            Tag = sessionInfo;
        }

        private async void btnActivateLicense_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLicenseKey.Text))
            {
                ShowMessage("Please enter a license key.", "Validation Error", MessageBoxIcon.Warning);
                return;
            }

            var sessionInfo = Tag as SessionManager.SessionInfo;
            if (sessionInfo == null)
            {
                ShowMessage("Session expired. Please login again.", "Error", MessageBoxIcon.Error);
                return;
            }

            SetControlsEnabled(false);
            lblStatus.Text = "Activating license...";
            lblStatus.ForeColor = Color.Yellow;

            try
            {
                var response = await RemoteLicenseManager.Instance.ActivateLicenseAsync(
                    sessionInfo.Username, txtLicenseKey.Text.Trim(), sessionInfo.SessionToken);

                if (response.Success && response.Data != null)
                {
                    LicenseInfo = response.Data;
                    LicenseManager.Instance.SetCurrentLicense(response.Data);

                    // Update stored session with license key
                    sessionInfo.LicenseKey = response.Data.LicenseKey;
                    SessionManager.StoreSession(sessionInfo);

                    lblStatus.Text = "License activated successfully!";
                    lblStatus.ForeColor = Color.Green;

                    await Task.Delay(1000);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowMessage(response.Error ?? "License activation failed.", "Activation Failed", MessageBoxIcon.Error);
                    lblStatus.Text = "License activation failed.";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"An error occurred during license activation: {ex.Message}", "Error", MessageBoxIcon.Error);
                lblStatus.Text = "Activation error.";
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        private async void btnPurchaseLicense_Click(object sender, EventArgs e)
        {
            var sessionInfo = Tag as SessionManager.SessionInfo;
            if (sessionInfo == null)
            {
                ShowMessage("Session expired. Please login again.", "Error", MessageBoxIcon.Error);
                return;
            }

            LicenseType selectedType;
            if (radioBtn3Month.Checked)
                selectedType = LicenseType.ThreeMonth;
            else if (radioBtn6Month.Checked)
                selectedType = LicenseType.SixMonth;
            else if (radioBtn12Month.Checked)
                selectedType = LicenseType.TwelveMonth;
            else
            {
                ShowMessage("Please select a license type.", "Validation Error", MessageBoxIcon.Warning);
                return;
            }

            SetControlsEnabled(false);
            lblStatus.Text = "Processing license purchase...";
            lblStatus.ForeColor = Color.Yellow;

            try
            {
                var response = await RemoteLicenseManager.Instance.PurchaseLicenseAsync(
                    sessionInfo.Username, selectedType, sessionInfo.SessionToken);

                if (response.Success && response.Data != null)
                {
                    LicenseInfo = response.Data;
                    LicenseManager.Instance.SetCurrentLicense(response.Data);

                    // Update stored session with license key
                    sessionInfo.LicenseKey = response.Data.LicenseKey;
                    SessionManager.StoreSession(sessionInfo);

                    ShowMessage($"License purchased successfully!\nLicense Key: {response.Data.LicenseKey}\nExpiry Date: {response.Data.ExpiryDate:yyyy-MM-dd}",
                        "Purchase Successful", MessageBoxIcon.Information);

                    lblStatus.Text = "License purchased successfully!";
                    lblStatus.ForeColor = Color.Green;

                    await Task.Delay(1000);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowMessage(response.Error ?? "License purchase failed.", "Purchase Failed", MessageBoxIcon.Error);
                    lblStatus.Text = "License purchase failed.";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"An error occurred during license purchase: {ex.Message}", "Error", MessageBoxIcon.Error);
                lblStatus.Text = "Purchase error.";
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                SetControlsEnabled(true);
            }
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_isRegistering)
            {
                SwitchToLoginMode();
            }
            else
            {
                SwitchToRegisterMode();
            }
        }

        private void SwitchToLoginMode()
        {
            _isRegistering = false;
            lblTitle.Text = "Iniciar Sesión - CipherUnlock Pro V1";
            btnLogin.Text = "Iniciar Sesión";
            lnkRegister.Text = "¿No tienes cuenta? Regístrate aquí";
            txtEmail.Visible = false;
            txtConfirmPassword.Visible = false;
            lblEmail.Visible = false;
            lblConfirmPassword.Visible = false;
            panelLicense.Visible = false;
            lblStatus.Text = "";
            ClearFields();
        }

        private void SwitchToRegisterMode()
        {
            _isRegistering = true;
            lblTitle.Text = "Registro - CipherUnlock Pro V1";
            btnLogin.Text = "Registrarse";
            lnkRegister.Text = "¿Ya tienes cuenta? Inicia sesión aquí";
            txtEmail.Visible = true;
            txtConfirmPassword.Visible = true;
            lblEmail.Visible = true;
            lblConfirmPassword.Visible = true;
            panelLicense.Visible = false;
            lblStatus.Text = "";
            ClearFields();
        }

        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            txtConfirmPassword.Clear();
            txtLicenseKey.Clear();
        }

        private void SetControlsEnabled(bool enabled)
        {
            txtUsername.Enabled = enabled;
            txtPassword.Enabled = enabled;
            txtEmail.Enabled = enabled;
            txtConfirmPassword.Enabled = enabled;
            txtLicenseKey.Enabled = enabled;
            btnLogin.Enabled = enabled;
            btnActivateLicense.Enabled = enabled;
            btnPurchaseLicense.Enabled = enabled;
            lnkRegister.Enabled = enabled;
            radioBtn3Month.Enabled = enabled;
            radioBtn6Month.Enabled = enabled;
            radioBtn12Month.Enabled = enabled;
        }

        private void ShowMessage(string message, string title, MessageBoxIcon icon)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        private void StartupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                Application.Exit();
            }
        }
    }
}