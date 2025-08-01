using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CipherUnlockProV1.Licensing;
using HtmlAgilityPack;
using System.Net.Http;

namespace CipherUnlockProV1
{
    public partial class Form1 : Form
    {
        private readonly LicenseInfo _licenseInfo;
        private SessionManager.SessionInfo _currentSession;
        private System.Windows.Forms.Timer _licenseCheckTimer;

        // Variables para ADB y manejo de dispositivos
        private void SetupCustomCommandPlaceholder()
        {
            txtCustomCommand.ForeColor = Color.Gray;
            txtCustomCommand.Text = "Ingresa comando personalizado...";

            txtCustomCommand.Enter += (s, e) => {
                if (txtCustomCommand.Text == "Ingresa comando personalizado...")
                {
                    txtCustomCommand.Text = "";
                    txtCustomCommand.ForeColor = Color.White;
                }
            };

            txtCustomCommand.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtCustomCommand.Text))
                {
                    txtCustomCommand.Text = "Ingresa comando personalizado...";
                    txtCustomCommand.ForeColor = Color.Gray;
                }
            };
        }
        private Process adbProcess;
        private string selectedDevice = "";
        private List<string> availableDevices = new List<string>();
        private Dictionary<string, DeviceInfo> deviceInfoCache = new Dictionary<string, DeviceInfo>();

        // Variables para puertos COM y Qualcomm
        private SerialPort comPort;
        private string selectedComPort = "";
        private List<string> availableComPorts = new List<string>();
        private Dictionary<string, ComPortInfo> comPortCache = new Dictionary<string, ComPortInfo>();

        // Variables para operaciones de desbloqueo
        private bool isUnlockingInProgress = false;
        private CancellationTokenSource cancellationTokenSource;

        // Variables para HTMLAgilityPack y web scraping
        private HttpClient httpClient;

        // Estructuras de datos para información de dispositivos
        public class DeviceInfo
        {
            public string DeviceId { get; set; }
            public string Manufacturer { get; set; }
            public string Model { get; set; }
            public string AndroidVersion { get; set; }
            public string BuildNumber { get; set; }
            public string SerialNumber { get; set; }
            public string Bootloader { get; set; }
            public bool IsRooted { get; set; }
            public bool IsBootloaderUnlocked { get; set; }
            public string SocType { get; set; }
            public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
        }

        public class ComPortInfo
        {
            public string PortName { get; set; }
            public string Description { get; set; }
            public string Manufacturer { get; set; }
            public bool IsQualcommDevice { get; set; }
            public bool IsMediaTekDevice { get; set; }
            public string DeviceType { get; set; }
        }

        public Form1(LicenseInfo licenseInfo)
        {
            InitializeComponent();
            _licenseInfo = licenseInfo;
            _currentSession = SessionManager.GetStoredSession();

            SetupForm();
            InitializeLicenseMonitoring();
            InitializeDeviceManagement();
            InitializeUnlockingComponents();
        }

        private void SetupForm()
        {
            this.Text = "CipherUnlock Pro V1 - Herramienta Profesional de Desbloqueo";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(45, 45, 48);

            UpdateLicenseDisplay();
            SetControlsBasedOnLicense();
        }

        private void InitializeLicenseMonitoring()
        {
            _licenseCheckTimer = new System.Windows.Forms.Timer();
            _licenseCheckTimer.Interval = 300000; // 5 minutos
            _licenseCheckTimer.Tick += LicenseCheckTimer_Tick;
            _licenseCheckTimer.Start();
        }

        private void InitializeDeviceManagement()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);
           

            CheckAdbAvailability();
            CheckFastbootAvailability();
            RefreshComPorts();
            RefreshAdbDevices();
        }

        private void InitializeUnlockingComponents()
        {
            cancellationTokenSource = new CancellationTokenSource();

            cmbDevices.SelectedIndexChanged += CmbDevices_SelectedIndexChanged;
            cmbComPorts.SelectedIndexChanged += CmbComPorts_SelectedIndexChanged;

            btnRefreshDevices.Click += BtnRefreshDevices_Click;
            btnRefreshComPorts.Click += BtnRefreshComPorts_Click;
            btnUnlockDevice.Click += BtnUnlockDevice_Click;
            btnQualcommUnlock.Click += BtnQualcommUnlock_Click;
            btnCancelOperation.Click += BtnCancelOperation_Click;
            btnAdvancedUnlock.Click += BtnAdvancedUnlock_Click;
            btnExecuteCommand.Click += BtnExecuteCommand_Click;
            btnClearLog.Click += (s, e) => txtLog.Clear();
            btnSaveLog.Click += BtnSaveLog_Click;
        }

        #region ===== FUNCIONES COMPLETAS DE ADB =====

        private void CheckAdbAvailability()
        {
            try
            {
                var result = ExecuteCommand("adb", "version", 5000);
                if (result.Success)
                {
                    lblAdbStatus.Text = "Estado ADB: ✓ Disponible";
                    lblAdbStatus.ForeColor = Color.Green;
                    LogMessage("ADB detectado y funcionando correctamente.");

                    // Verificar ADB server
                    StartAdbServer();
                }
                else
                {
                    lblAdbStatus.Text = "Estado ADB: ✗ No disponible";
                    lblAdbStatus.ForeColor = Color.Red;
                    LogError("ADB no está disponible. Instala Android SDK Platform Tools.");
                }
            }
            catch (Exception ex)
            {
                lblAdbStatus.Text = "Estado ADB: ✗ Error";
                lblAdbStatus.ForeColor = Color.Red;
                LogError($"Error verificando ADB: {ex.Message}");
            }
        }

        private void StartAdbServer()
        {
            try
            {
                ExecuteCommand("adb", "start-server", 10000);
                LogMessage("Servidor ADB iniciado.");
            }
            catch (Exception ex)
            {
                LogError($"Error iniciando servidor ADB: {ex.Message}");
            }
        }

        private void RefreshAdbDevices()
        {
            try
            {
                availableDevices.Clear();
                deviceInfoCache.Clear();
                cmbDevices.Items.Clear();

                var result = ExecuteCommand("adb", "devices -l", 10000);
                if (!result.Success)
                {
                    LogError("Error obteniendo lista de dispositivos ADB.");
                    return;
                }

                var lines = result.Output.Split('\n');
                foreach (var line in lines)
                {
                    if (line.Contains("\t") && (line.Contains("device") || line.Contains("recovery") || line.Contains("bootloader")))
                    {
                        var parts = line.Split('\t');
                        if (parts.Length >= 2)
                        {
                            string deviceId = parts[0].Trim();
                            string status = parts[1].Trim().Split(' ')[0];

                            availableDevices.Add(deviceId);
                            cmbDevices.Items.Add($"{deviceId} ({status})");

                            // Obtener información detallada del dispositivo
                            Task.Run(() => GetDetailedDeviceInfo(deviceId));
                        }
                    }
                }

                lblDeviceCount.Text = $"Dispositivos ADB: {availableDevices.Count}";

                if (availableDevices.Count > 0)
                {
                    cmbDevices.SelectedIndex = 0;
                    LogMessage($"Se detectaron {availableDevices.Count} dispositivos ADB.");
                }
                else
                {
                    LogMessage("No se detectaron dispositivos ADB. Verifica la conexión USB y habilita depuración USB.");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error escaneando dispositivos ADB: {ex.Message}");
            }
        }

        private async Task GetDetailedDeviceInfo(string deviceId)
        {
            try
            {
                var deviceInfo = new DeviceInfo { DeviceId = deviceId };

                // Obtener propiedades básicas
                var propsResult = await ExecuteCommandAsync("adb", $"-s {deviceId} shell getprop", 15000);
                if (propsResult.Success)
                {
                    ParseDeviceProperties(deviceInfo, propsResult.Output);
                }

                // Verificar si está rooteado
                var rootResult = await ExecuteCommandAsync("adb", $"-s {deviceId} shell su -c 'id'", 5000);
                deviceInfo.IsRooted = rootResult.Success && rootResult.Output.Contains("uid=0");

                // Verificar bootloader
                await CheckBootloaderStatus(deviceInfo);

                deviceInfoCache[deviceId] = deviceInfo;

                // Actualizar UI en el hilo principal
                this.Invoke(new Action(() => UpdateDeviceInfoDisplay(deviceInfo)));
            }
            catch (Exception ex)
            {
                LogError($"Error obteniendo información detallada del dispositivo {deviceId}: {ex.Message}");
            }
        }

        private void ParseDeviceProperties(DeviceInfo deviceInfo, string propOutput)
        {
            var lines = propOutput.Split('\n');
            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"\[(.*?)\]: \[(.*?)\]");
                if (match.Success)
                {
                    string key = match.Groups[1].Value;
                    string value = match.Groups[2].Value;

                    deviceInfo.Properties[key] = value;

                    switch (key)
                    {
                        case "ro.product.manufacturer":
                            deviceInfo.Manufacturer = value;
                            break;
                        case "ro.product.model":
                            deviceInfo.Model = value;
                            break;
                        case "ro.build.version.release":
                            deviceInfo.AndroidVersion = value;
                            break;
                        case "ro.build.display.id":
                            deviceInfo.BuildNumber = value;
                            break;
                        case "ro.serialno":
                            deviceInfo.SerialNumber = value;
                            break;
                        case "ro.bootloader":
                            deviceInfo.Bootloader = value;
                            break;
                        case "ro.hardware":
                        case "ro.board.platform":
                            deviceInfo.SocType = value;
                            break;
                    }
                }
            }
        }

        private async Task CheckBootloaderStatus(DeviceInfo deviceInfo)
        {
            try
            {
                // Reiniciar en bootloader
                var rebootResult = await ExecuteCommandAsync("adb", $"-s {deviceInfo.DeviceId} reboot bootloader", 5000);
                if (rebootResult.Success)
                {
                    await Task.Delay(8000); // Esperar a que reinicie

                    // Verificar estado del bootloader
                    var unlockResult = await ExecuteCommandAsync("fastboot", "getvar unlocked", 5000);
                    if (unlockResult.Success)
                    {
                        deviceInfo.IsBootloaderUnlocked = unlockResult.Output.Contains("unlocked: yes");
                    }

                    // Regresar al sistema
                    await ExecuteCommandAsync("fastboot", "reboot", 3000);
                }
            }
            catch (Exception ex)
            {
                LogError($"Error verificando estado del bootloader: {ex.Message}");
            }
        }

        // Funciones ADB específicas
        private async Task<CommandResult> AdbShellCommand(string deviceId, string command)
        {
            return await ExecuteCommandAsync("adb", $"-s {deviceId} shell {command}", 15000);
        }

        private async Task<CommandResult> AdbPushFile(string deviceId, string localPath, string remotePath)
        {
            return await ExecuteCommandAsync("adb", $"-s {deviceId} push \"{localPath}\" \"{remotePath}\"", 30000);
        }

        private async Task<CommandResult> AdbPullFile(string deviceId, string remotePath, string localPath)
        {
            return await ExecuteCommandAsync("adb", $"-s {deviceId} pull \"{remotePath}\" \"{localPath}\"", 30000);
        }

        private async Task<CommandResult> AdbInstallApk(string deviceId, string apkPath)
        {
            return await ExecuteCommandAsync("adb", $"-s {deviceId} install \"{apkPath}\"", 60000);
        }

        private async Task<CommandResult> AdbReboot(string deviceId, string mode = "")
        {
            string args = string.IsNullOrEmpty(mode) ? $"-s {deviceId} reboot" : $"-s {deviceId} reboot {mode}";
            return await ExecuteCommandAsync("adb", args, 10000);
        }

        #endregion

        #region ===== FUNCIONES COMPLETAS DE FASTBOOT =====

        private void CheckFastbootAvailability()
        {
            try
            {
                var result = ExecuteCommand("fastboot", "--version", 5000);
                if (result.Success)
                {
                    lblFastbootStatus.Text = "Estado Fastboot: ✓ Disponible";
                    lblFastbootStatus.ForeColor = Color.Green;
                    LogMessage("Fastboot detectado y funcionando correctamente.");
                }
                else
                {
                    lblFastbootStatus.Text = "Estado Fastboot: ✗ No disponible";
                    lblFastbootStatus.ForeColor = Color.Red;
                    LogError("Fastboot no está disponible. Instala Android SDK Platform Tools.");
                }
            }
            catch (Exception ex)
            {
                lblFastbootStatus.Text = "Estado Fastboot: ✗ Error";
                lblFastbootStatus.ForeColor = Color.Red;
                LogError($"Error verificando Fastboot: {ex.Message}");
            }
        }

        // Funciones Fastboot específicas
        private async Task<CommandResult> FastbootGetVar(string variable)
        {
            return await ExecuteCommandAsync("fastboot", $"getvar {variable}", 10000);
        }

        private async Task<CommandResult> FastbootUnlockBootloader()
        {
            LogMessage("Iniciando desbloqueo del bootloader...");
            LogMessage("ADVERTENCIA: Esto borrará todos los datos del dispositivo!");

            var result = await ExecuteCommandAsync("fastboot", "flashing unlock", 30000);
            if (result.Success)
            {
                LogMessage("Comando de desbloqueo enviado. Confirma en el dispositivo.");
            }
            return result;
        }

        private async Task<CommandResult> FastbootUnlockCritical()
        {
            LogMessage("Desbloqueando particiones críticas...");
            return await ExecuteCommandAsync("fastboot", "flashing unlock_critical", 30000);
        }

        private async Task<CommandResult> FastbootFlashPartition(string partition, string imagePath)
        {
            LogMessage($"Flasheando partición {partition}...");
            return await ExecuteCommandAsync("fastboot", $"flash {partition} \"{imagePath}\"", 120000);
        }

        private async Task<CommandResult> FastbootErasePartition(string partition)
        {
            LogMessage($"Borrando partición {partition}...");
            return await ExecuteCommandAsync("fastboot", $"erase {partition}", 30000);
        }

        private async Task<CommandResult> FastbootFormatPartition(string partition, string filesystem = "ext4")
        {
            LogMessage($"Formateando partición {partition} como {filesystem}...");
            return await ExecuteCommandAsync("fastboot", $"format:{filesystem} {partition}", 60000);
        }

        private async Task<CommandResult> FastbootOemUnlock()
        {
            LogMessage("Ejecutando comando OEM unlock...");
            return await ExecuteCommandAsync("fastboot", "oem unlock", 30000);
        }

        private async Task<CommandResult> FastbootReboot(string mode = "")
        {
            string args = string.IsNullOrEmpty(mode) ? "reboot" : $"reboot-{mode}";
            return await ExecuteCommandAsync("fastboot", args, 10000);
        }

        private async Task<List<string>> GetFastbootDevices()
        {
            var devices = new List<string>();
            var result = await ExecuteCommandAsync("fastboot", "devices", 5000);

            if (result.Success)
            {
                var lines = result.Output.Split('\n');
                foreach (var line in lines)
                {
                    if (line.Contains("\t"))
                    {
                        var deviceId = line.Split('\t')[0].Trim();
                        if (!string.IsNullOrEmpty(deviceId))
                        {
                            devices.Add(deviceId);
                        }
                    }
                }
            }

            return devices;
        }

        #endregion

        #region ===== FUNCIONES COMPLETAS DE QUALCOMM/COM =====

        private void RefreshComPorts()
        {
            try
            {
                availableComPorts.Clear();
                comPortCache.Clear();
                cmbComPorts.Items.Clear();

                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    availableComPorts.Add(port);
                    var portInfo = GetComPortInfo(port);
                    comPortCache[port] = portInfo;

                    string displayName = $"{port}";
                    if (!string.IsNullOrEmpty(portInfo.Description))
                    {
                        displayName += $" - {portInfo.Description}";
                    }

                    cmbComPorts.Items.Add(displayName);
                }

                lblComPortCount.Text = $"Puertos COM: {availableComPorts.Count}";

                if (availableComPorts.Count > 0)
                {
                    cmbComPorts.SelectedIndex = 0;
                    LogMessage($"Se detectaron {availableComPorts.Count} puertos COM.");
                }
                else
                {
                    LogMessage("No se detectaron puertos COM disponibles.");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error escaneando puertos COM: {ex.Message}");
            }
        }

        private ComPortInfo GetComPortInfo(string portName)
        {
            var portInfo = new ComPortInfo { PortName = portName };

            try
            {
                using (var searcher = new System.Management.ManagementObjectSearcher(
                    $"SELECT * FROM Win32_PnPEntity WHERE Caption LIKE '%{portName}%'"))
                {
                    foreach (var obj in searcher.Get())
                    {
                        portInfo.Description = obj["Caption"]?.ToString() ?? "";
                        portInfo.Manufacturer = obj["Manufacturer"]?.ToString() ?? "";

                        // Detectar tipo de dispositivo
                        string caption = portInfo.Description.ToLower();
                        if (caption.Contains("qualcomm") || caption.Contains("qcom"))
                        {
                            portInfo.IsQualcommDevice = true;
                            portInfo.DeviceType = "Qualcomm";
                        }
                        else if (caption.Contains("mediatek") || caption.Contains("mtk"))
                        {
                            portInfo.IsMediaTekDevice = true;
                            portInfo.DeviceType = "MediaTek";
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error obteniendo información del puerto {portName}: {ex.Message}");
            }

            return portInfo;
        }

        private bool OpenComPort(string portName, int baudRate = 115200)
        {
            try
            {
                if (comPort != null && comPort.IsOpen)
                {
                    comPort.Close();
                    comPort.Dispose();
                }

                comPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One)
                {
                    Handshake = Handshake.None,
                    ReadTimeout = 5000,
                    WriteTimeout = 5000,
                    NewLine = "\r\n"
                };

                comPort.Open();
                LogMessage($"Puerto {portName} abierto exitosamente a {baudRate} baudios.");
                return true;
            }
            catch (Exception ex)
            {
                LogError($"Error abriendo puerto {portName}: {ex.Message}");
                return false;
            }
        }

        private void CloseComPort()
        {
            try
            {
                if (comPort != null && comPort.IsOpen)
                {
                    comPort.Close();
                    comPort.Dispose();
                    comPort = null;
                    LogMessage("Puerto COM cerrado.");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error cerrando puerto COM: {ex.Message}");
            }
        }

        // Comandos AT básicos
        private async Task<string> SendATCommand(string command, int timeoutMs = 3000)
        {
            if (comPort == null || !comPort.IsOpen)
            {
                LogError("Puerto COM no está abierto.");
                return "";
            }

            try
            {
                // Limpiar buffer
                comPort.DiscardInBuffer();
                comPort.DiscardOutBuffer();

                // Enviar comando
                comPort.WriteLine(command);
                LogMessage($"Enviado: {command}");

                // Esperar respuesta
                var startTime = DateTime.Now;
                var response = new StringBuilder();

                while ((DateTime.Now - startTime).TotalMilliseconds < timeoutMs)
                {
                    if (comPort.BytesToRead > 0)
                    {
                        string data = comPort.ReadExisting();
                        response.Append(data);

                        if (data.Contains("OK") || data.Contains("ERROR"))
                        {
                            break;
                        }
                    }
                    await Task.Delay(100);
                }

                string result = response.ToString().Trim();
                LogMessage($"Respuesta: {result}");
                return result;
            }
            catch (Exception ex)
            {
                LogError($"Error enviando comando AT: {ex.Message}");
                return "";
            }
        }

        // Comandos Qualcomm específicos
        private async Task<bool> QualcommEnterDiagMode()
        {
            LogMessage("Intentando entrar en modo diagnóstico Qualcomm...");

            // Probar diferentes comandos de diagnóstico
            string[] diagCommands = {
                "AT$QCDMG",
                "AT!ENTERCND=\"A710\"",
                "AT$DIAGNOSTIC",
                "AT+CFUN=1,1"
            };

            foreach (string cmd in diagCommands)
            {
                var response = await SendATCommand(cmd);
                if (response.Contains("OK"))
                {
                    LogMessage($"Modo diagnóstico activado con comando: {cmd}");
                    return true;
                }
            }

            LogError("No se pudo entrar en modo diagnóstico.");
            return false;
        }

        private async Task<bool> QualcommUnlockBootloader()
        {
            LogMessage("Iniciando desbloqueo de bootloader Qualcomm...");

            // Comandos de desbloqueo específicos de Qualcomm
            string[] unlockCommands = {
                "AT!UNLOCK=\"A710\"",
                "AT!CUSTOM=\"SIMLOCK\",0",
                "AT+CBOOTLDR=0",
                "AT!BOOTHOLD",
                "AT!UNLOCK=\"\"",
                "AT!ENTERCND=\"A710\""
            };

            foreach (string cmd in unlockCommands)
            {
                var response = await SendATCommand(cmd, 10000);
                await Task.Delay(1000);

                if (response.Contains("OK"))
                {
                    LogMessage($"Comando ejecutado exitosamente: {cmd}");
                }
                else
                {
                    LogMessage($"Comando falló: {cmd} - Respuesta: {response}");
                }
            }

            LogMessage("Secuencia de desbloqueo Qualcomm completada.");
            return true;
        }

        private async Task<Dictionary<string, string>> GetQualcommDeviceInfo()
        {
            var info = new Dictionary<string, string>();

            var commands = new Dictionary<string, string>
            {
                ["Fabricante"] = "AT+CGMI",
                ["Modelo"] = "AT+CGMM",
                ["Revisión"] = "AT+CGMR",
                ["IMEI"] = "AT+CGSN",
                ["Número de Serie"] = "AT+CGSN=1",
                ["Versión de Software"] = "ATI",
                ["Estado SIM"] = "AT+CPIN?",
                ["Operador"] = "AT+COPS?"
            };

            foreach (var cmd in commands)
            {
                var response = await SendATCommand(cmd.Value);
                if (!string.IsNullOrEmpty(response) && !response.Contains("ERROR"))
                {
                    info[cmd.Key] = response;
                }
            }

            return info;
        }

        // Comandos MediaTek específicos
        private async Task<bool> MediaTekUnlock()
        {
            LogMessage("Iniciando desbloqueo MediaTek...");

            string[] mtkCommands = {
                "AT+EBOOT=1,1",
                "AT+ESLP=0",
                "AT+EFUN=0",
                "AT+ECHARGE=0",
                "AT+EAUTH",
                "AT+ESUO=3"
            };

            foreach (string cmd in mtkCommands)
            {
                var response = await SendATCommand(cmd, 5000);
                await Task.Delay(500);

                if (response.Contains("OK"))
                {
                    LogMessage($"Comando MTK exitoso: {cmd}");
                }
            }

            return true;
        }

        #endregion

        #region ===== OPERACIONES DE DESBLOQUEO =====

        private async void BtnUnlockDevice_Click(object sender, EventArgs e)
        {
            if (!ValidateLicenseForOperation()) return;

            if (string.IsNullOrEmpty(selectedDevice))
            {
                MessageBox.Show("Por favor selecciona un dispositivo ADB.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await PerformCompleteAdbUnlock();
        }

        private async Task PerformCompleteAdbUnlock()
        {
            isUnlockingInProgress = true;
            SetUnlockingState(true);

            try
            {
                LogMessage($"=== INICIANDO DESBLOQUEO COMPLETO PARA {selectedDevice} ===");
                progressBarUnlock.Value = 0;

                // Paso 1: Verificar conexión del dispositivo
                LogMessage("Paso 1/10: Verificando conexión del dispositivo...");
                var deviceCheck = await ExecuteCommandAsync("adb", $"-s {selectedDevice} get-state", 5000);
                if (!deviceCheck.Success || !deviceCheck.Output.Contains("device"))
                {
                    LogError("Dispositivo no conectado o no responde.");
                    return;
                }
                progressBarUnlock.Value = 10;

                // Paso 2: Habilitar OEM unlocking
                LogMessage("Paso 2/10: Habilitando OEM unlocking...");
                await AdbShellCommand(selectedDevice, "settings put global development_settings_enabled 1");
                await AdbShellCommand(selectedDevice, "settings put secure install_non_market_apps 1");
                await AdbShellCommand(selectedDevice, "settings put global oem_unlock_supported 1");
                progressBarUnlock.Value = 20;

                // Paso 3: Verificar bootloader
                LogMessage("Paso 3/10: Verificando estado del bootloader...");
                var rebootResult = await AdbReboot(selectedDevice, "bootloader");
                await Task.Delay(10000); // Esperar reinicio
                progressBarUnlock.Value = 30;

                // Paso 4: Verificar dispositivo en fastboot
                LogMessage("Paso 4/10: Verificando modo fastboot...");
                var fastbootDevices = await GetFastbootDevices();
                if (fastbootDevices.Count == 0)
                {
                    LogError("Dispositivo no detectado en modo fastboot.");
                    return;
                }
                progressBarUnlock.Value = 40;

                // Paso 5: Verificar estado actual del bootloader
                LogMessage("Paso 5/10: Verificando estado de desbloqueo...");
                var unlockStatus = await FastbootGetVar("unlocked");
                bool isAlreadyUnlocked = unlockStatus.Success && unlockStatus.Output.Contains("unlocked: yes");

                if (isAlreadyUnlocked)
                {
                    LogMessage("¡El bootloader ya está desbloqueado!");
                    progressBarUnlock.Value = 100;
                    await FastbootReboot();
                    MessageBox.Show("El dispositivo ya está desbloqueado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                progressBarUnlock.Value = 50;

                // Paso 6: Obtener información del dispositivo
                LogMessage("Paso 6/10: Obteniendo información del dispositivo...");
                var productResult = await FastbootGetVar("product");
                var versionResult = await FastbootGetVar("version-bootloader");
                LogMessage($"Producto: {productResult.Output}");
                LogMessage($"Versión bootloader: {versionResult.Output}");
                progressBarUnlock.Value = 60;

                // Paso 7: Intentar desbloqueo estándar
                LogMessage("Paso 7/10: Intentando desbloqueo estándar...");
                var unlockResult = await FastbootUnlockBootloader();
                await Task.Delay(5000); // Tiempo para que el usuario confirme
                progressBarUnlock.Value = 70;

                // Paso 8: Verificar si el desbloqueo funcionó
                LogMessage("Paso 8/10: Verificando resultado del desbloqueo...");
                var checkResult = await FastbootGetVar("unlocked");
                bool unlockSucceeded = checkResult.Success && checkResult.Output.Contains("unlocked: yes");
                progressBarUnlock.Value = 80;

                // Paso 9: Intentar métodos alternativos si falló
                if (!unlockSucceeded)
                {
                    LogMessage("Paso 9/10: Probando métodos alternativos...");

                    // Probar OEM unlock
                    await FastbootOemUnlock();
                    await Task.Delay(3000);

                    // Probar unlock critical
                    await FastbootUnlockCritical();
                    await Task.Delay(3000);

                    // Verificar nuevamente
                    checkResult = await FastbootGetVar("unlocked");
                    unlockSucceeded = checkResult.Success && checkResult.Output.Contains("unlocked: yes");
                }
                progressBarUnlock.Value = 90;

                // Paso 10: Finalización
                LogMessage("Paso 10/10: Finalizando proceso...");
                await FastbootReboot();
                progressBarUnlock.Value = 100;

                if (unlockSucceeded)
                {
                    LogMessage("=== ¡DESBLOQUEO EXITOSO! ===");
                    MessageBox.Show("¡Bootloader desbloqueado exitosamente!\n\nEl dispositivo se reiniciará y realizará un factory reset.",
                        "Desbloqueo Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    LogMessage("=== DESBLOQUEO FALLIDO ===");
                    MessageBox.Show("El desbloqueo no se completó exitosamente.\n\nPosibles causas:\n- OEM unlocking no habilitado\n- Bootloader no desbloqueabl",
                        "Desbloqueo Fallido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LogError($"Error durante el desbloqueo: {ex.Message}");
                MessageBox.Show($"Error crítico durante el desbloqueo:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isUnlockingInProgress = false;
                SetUnlockingState(false);
            }
        }

        private async void BtnQualcommUnlock_Click(object sender, EventArgs e)
        {
            if (!ValidateLicenseForOperation()) return;

            if (string.IsNullOrEmpty(selectedComPort))
            {
                MessageBox.Show("Por favor selecciona un puerto COM.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await PerformCompleteQualcommUnlock();
        }

        private async Task PerformCompleteQualcommUnlock()
        {
            isUnlockingInProgress = true;
            SetUnlockingState(true);

            try
            {
                LogMessage($"=== INICIANDO DESBLOQUEO QUALCOMM EN {selectedComPort} ===");
                progressBarUnlock.Value = 0;

                // Paso 1: Abrir puerto COM
                LogMessage("Paso 1/8: Abriendo puerto COM...");
                if (!OpenComPort(selectedComPort, 115200))
                {
                    LogError("No se pudo abrir el puerto COM.");
                    return;
                }
                progressBarUnlock.Value = 12;

                // Paso 2: Probar comunicación básica
                LogMessage("Paso 2/8: Probando comunicación básica...");
                var atResponse = await SendATCommand("AT");
                if (!atResponse.Contains("OK"))
                {
                    LogMessage("Respuesta AT básica falló, probando diferentes baudrates...");

                    // Probar diferentes velocidades
                    int[] baudRates = { 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 };
                    bool connected = false;

                    foreach (int baud in baudRates)
                    {
                        CloseComPort();
                        if (OpenComPort(selectedComPort, baud))
                        {
                            var testResponse = await SendATCommand("AT");
                            if (testResponse.Contains("OK"))
                            {
                                LogMessage($"Comunicación establecida a {baud} baudios.");
                                connected = true;
                                break;
                            }
                        }
                    }

                    if (!connected)
                    {
                        LogError("No se pudo establecer comunicación con el dispositivo.");
                        return;
                    }
                }
                progressBarUnlock.Value = 25;

                // Paso 3: Obtener información del dispositivo
                LogMessage("Paso 3/8: Obteniendo información del dispositivo...");
                var deviceInfo = await GetQualcommDeviceInfo();
                foreach (var info in deviceInfo)
                {
                    LogMessage($"{info.Key}: {info.Value}");
                }
                progressBarUnlock.Value = 37;

                // Paso 4: Entrar en modo diagnóstico
                LogMessage("Paso 4/8: Entrando en modo diagnóstico...");
                bool diagMode = await QualcommEnterDiagMode();
                if (!diagMode)
                {
                    LogMessage("Advertencia: No se pudo confirmar el modo diagnóstico, continuando...");
                }
                progressBarUnlock.Value = 50;

                // Paso 5: Ejecutar comandos de desbloqueo
                LogMessage("Paso 5/8: Ejecutando secuencia de desbloqueo...");
                await QualcommUnlockBootloader();
                progressBarUnlock.Value = 62;

                // Paso 6: Comandos específicos por fabricante
                LogMessage("Paso 6/8: Ejecutando comandos específicos...");
                var manufacturer = deviceInfo.ContainsKey("Fabricante") ? deviceInfo["Fabricante"].ToLower() : "";

                if (manufacturer.Contains("samsung"))
                {
                    await SendATCommand("AT+MODE=3");
                    await SendATCommand("AT+SBOOTLDR=0");
                }
                else if (manufacturer.Contains("lg"))
                {
                    await SendATCommand("AT%UNLOCK=\"LG_UNLOCK_CODE\"");
                    await SendATCommand("AT+CGMR");
                }
                else if (manufacturer.Contains("htc"))
                {
                    await SendATCommand("AT+BOOTLOADERUNLOCK");
                    await SendATCommand("AT+HTCUNLOCK");
                }
                progressBarUnlock.Value = 75;

                // Paso 7: Verificar estado final
                LogMessage("Paso 7/8: Verificando estado final...");
                await SendATCommand("AT+CPIN?");
                await SendATCommand("AT+CGMI");
                progressBarUnlock.Value = 87;

                // Paso 8: Reiniciar dispositivo
                LogMessage("Paso 8/8: Reiniciando dispositivo...");
                await SendATCommand("AT+CFUN=1,1");
                await Task.Delay(2000);
                progressBarUnlock.Value = 100;

                LogMessage("=== PROCESO QUALCOMM COMPLETADO ===");
                MessageBox.Show("El proceso de desbloqueo Qualcomm ha sido completado.\n\nVerifica el estado del dispositivo y reinícialo manualmente si es necesario.",
                    "Proceso Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogError($"Error durante el desbloqueo Qualcomm: {ex.Message}");
                MessageBox.Show($"Error durante el proceso Qualcomm:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseComPort();
                isUnlockingInProgress = false;
                SetUnlockingState(false);
            }
        }

        private async void BtnAdvancedUnlock_Click(object sender, EventArgs e)
        {
            if (!ValidateLicenseForOperation()) return;

            var advancedForm = new AdvancedUnlockForm();
            advancedForm.ShowDialog();
        }

        #endregion

        #region ===== FUNCIONES DE UTILIDAD =====

        public class CommandResult
        {
            public bool Success { get; set; }
            public string Output { get; set; }
            public string Error { get; set; }
            public int ExitCode { get; set; }
        }

        private CommandResult ExecuteCommand(string fileName, string arguments, int timeoutMs = 10000)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = fileName,
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        WorkingDirectory = Environment.CurrentDirectory
                    }
                };

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                bool finished = process.WaitForExit(timeoutMs);
                if (!finished)
                {
                    process.Kill();
                    return new CommandResult
                    {
                        Success = false,
                        Output = output,
                        Error = "Comando excedió el tiempo límite",
                        ExitCode = -1
                    };
                }

                return new CommandResult
                {
                    Success = process.ExitCode == 0,
                    Output = output,
                    Error = error,
                    ExitCode = process.ExitCode
                };
            }
            catch (Exception ex)
            {
                return new CommandResult
                {
                    Success = false,
                    Output = "",
                    Error = ex.Message,
                    ExitCode = -1
                };
            }
        }

        private async Task<CommandResult> ExecuteCommandAsync(string fileName, string arguments, int timeoutMs = 10000)
        {
            return await Task.Run(() => ExecuteCommand(fileName, arguments, timeoutMs));
        }

        private void UpdateDeviceInfoDisplay(DeviceInfo deviceInfo)
        {
            if (deviceInfo != null)
            {
                var info = $"{deviceInfo.Manufacturer} {deviceInfo.Model}";
                if (!string.IsNullOrEmpty(deviceInfo.AndroidVersion))
                {
                    info += $" (Android {deviceInfo.AndroidVersion})";
                }
                if (deviceInfo.IsRooted)
                {
                    info += " [ROOT]";
                }
                if (deviceInfo.IsBootloaderUnlocked)
                {
                    info += " [UNLOCKED]";
                }

                lblDeviceInfo.Text = info;
            }
        }

        private void BtnExecuteCommand_Click(object sender, EventArgs e)
        {
            if (!ValidateLicenseForOperation()) return;

            string command = txtCustomCommand.Text.Trim();
            if (string.IsNullOrEmpty(command))
            {
                MessageBox.Show("Ingresa un comando válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ExecuteCustomCommand(command);
        }

        private async void ExecuteCustomCommand(string command)
        {
            try
            {
                LogMessage($"Ejecutando comando personalizado: {command}");

                string[] parts = command.Split(' ', (char)2);
                string executable = parts[0];
                string arguments = parts.Length > 1 ? parts[1] : "";

                var result = await ExecuteCommandAsync(executable, arguments, 30000);

                if (result.Success)
                {
                    LogMessage($"Comando exitoso. Salida:\n{result.Output}");
                }
                else
                {
                    LogError($"Comando falló. Error:\n{result.Error}");
                }
            }
            catch (Exception ex)
            {
                LogError($"Error ejecutando comando personalizado: {ex.Message}");
            }
        }

        #endregion

        #region ===== EVENTOS Y FUNCIONES AUXILIARES =====

        private async void LicenseCheckTimer_Tick(object sender, EventArgs e)
        {
            await ValidateLicenseStatus();
        }

        private async Task ValidateLicenseStatus()
        {
            if (_currentSession == null) return;

            try
            {
                var response = await RemoteLicenseManager.Instance.GetLicenseInfoAsync(
                    _currentSession.Username, _currentSession.SessionToken);

                if (response.Success && response.Data != null)
                {
                    LicenseManager.Instance.SetCurrentLicense(response.Data);
                    UpdateLicenseDisplay();

                    if (!response.Data.IsValid)
                    {
                        ShowLicenseExpiredWarning();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"Error validando licencia: {ex.Message}");
            }
        }

        private void UpdateLicenseDisplay()
        {
            if (_licenseInfo != null && _currentSession != null)
            {
                lblUsername.Text = $"Usuario: {_currentSession.Username}";
                lblLicenseType.Text = $"Licencia: {_licenseInfo.LicenseType} meses";
                lblLicenseStatus.Text = $"Estado: {_licenseInfo.Status}";
                lblExpiryDate.Text = $"Vence: {_licenseInfo.ExpiryDate:dd/MM/yyyy}";
                lblDaysRemaining.Text = $"Días restantes: {_licenseInfo.DaysRemaining}";

                if (_licenseInfo.DaysRemaining <= 7)
                    lblDaysRemaining.ForeColor = Color.Red;
                else if (_licenseInfo.DaysRemaining <= 30)
                    lblDaysRemaining.ForeColor = Color.Orange;
                else
                    lblDaysRemaining.ForeColor = Color.Green;
            }
        }

        private void SetControlsBasedOnLicense()
        {
            bool hasValidLicense = _licenseInfo != null && _licenseInfo.IsValid;

            btnUnlockDevice.Enabled = hasValidLicense;
            btnQualcommUnlock.Enabled = hasValidLicense;
            btnAdvancedUnlock.Enabled = hasValidLicense;
            btnExecuteCommand.Enabled = hasValidLicense;
            tabControlUnlock.Enabled = hasValidLicense;

            if (!hasValidLicense)
            {
                lblStatusMessage.Text = "Licencia expirada o inválida. Las funciones están deshabilitadas.";
                lblStatusMessage.ForeColor = Color.Red;
            }
            else
            {
                lblStatusMessage.Text = "Listo para operaciones de desbloqueo.";
                lblStatusMessage.ForeColor = Color.Green;
            }
        }

        private void ShowLicenseExpiredWarning()
        {
            var result = MessageBox.Show(
                "Tu licencia ha expirado. ¿Deseas renovarla ahora?",
                "Licencia Expirada",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                ShowLicenseRenewalDialog();
            }
        }

        private void ShowLicenseRenewalDialog()
        {
            using (var renewalForm = new StartupForm())
            {
                if (renewalForm.ShowDialog() == DialogResult.OK)
                {
                    var newLicense = renewalForm.LicenseInfo;
                    LicenseManager.Instance.SetCurrentLicense(newLicense);
                    UpdateLicenseDisplay();
                    SetControlsBasedOnLicense();
                }
            }
        }

        private bool ValidateLicenseForOperation()
        {
            if (_licenseInfo == null || !_licenseInfo.IsValid)
            {
                MessageBox.Show("Tu licencia ha expirado o es inválida. No puedes realizar operaciones de desbloqueo.",
                    "Licencia Inválida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void CmbDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDevices.SelectedIndex >= 0)
            {
                selectedDevice = availableDevices[cmbDevices.SelectedIndex];
                LogMessage($"Dispositivo ADB seleccionado: {selectedDevice}");

                if (deviceInfoCache.ContainsKey(selectedDevice))
                {
                    UpdateDeviceInfoDisplay(deviceInfoCache[selectedDevice]);
                }
            }
        }

        private void CmbComPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbComPorts.SelectedIndex >= 0)
            {
                selectedComPort = availableComPorts[cmbComPorts.SelectedIndex];
                LogMessage($"Puerto COM seleccionado: {selectedComPort}");

                if (comPortCache.ContainsKey(selectedComPort))
                {
                    var portInfo = comPortCache[selectedComPort];
                    LogMessage($"Tipo de dispositivo: {portInfo.DeviceType}");
                }
            }
        }

        private void BtnRefreshDevices_Click(object sender, EventArgs e)
        {
            RefreshAdbDevices();
        }

        private void BtnRefreshComPorts_Click(object sender, EventArgs e)
        {
            RefreshComPorts();
        }

        private void BtnCancelOperation_Click(object sender, EventArgs e)
        {
            if (isUnlockingInProgress)
            {
                cancellationTokenSource.Cancel();
                LogMessage("=== OPERACIÓN CANCELADA POR EL USUARIO ===");
                SetUnlockingState(false);
                isUnlockingInProgress = false;
                CloseComPort();
            }
        }

        private void SetUnlockingState(bool isUnlocking)
        {
            btnUnlockDevice.Enabled = !isUnlocking && ValidateLicenseForOperation();
            btnQualcommUnlock.Enabled = !isUnlocking && ValidateLicenseForOperation();
            btnAdvancedUnlock.Enabled = !isUnlocking && ValidateLicenseForOperation();
            btnExecuteCommand.Enabled = !isUnlocking && ValidateLicenseForOperation();
            btnCancelOperation.Enabled = isUnlocking;
            btnRefreshDevices.Enabled = !isUnlocking;
            btnRefreshComPorts.Enabled = !isUnlocking;

            if (!isUnlocking)
            {
                progressBarUnlock.Value = 0;
            }
        }

        private void LogMessage(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => LogMessage(message)));
                return;
            }

            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            txtLog.AppendText($"[{timestamp}] {message}\r\n");
            txtLog.ScrollToCaret();
        }

        private void LogError(string error)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => LogError(error)));
                return;
            }

            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            txtLog.AppendText($"[{timestamp}] ❌ ERROR: {error}\r\n");
            txtLog.ScrollToCaret();
        }

        private void BtnSaveLog_Click(object sender, EventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*",
                    DefaultExt = "txt",
                    FileName = $"CipherUnlock_Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveDialog.FileName, txtLog.Text);
                    MessageBox.Show("Log guardado exitosamente.", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error guardando el log: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            LogMessage("=== CIPHER UNLOCK PRO V1 INICIADO ===");
            LogMessage($"Usuario: {_currentSession?.Username}");
            LogMessage($"Licencia: {_licenseInfo?.LicenseType} meses, vence: {_licenseInfo?.ExpiryDate:dd/MM/yyyy}");
            LogMessage($"Días restantes: {_licenseInfo?.DaysRemaining}");

            await Task.Delay(1000);
            CheckRequiredTools();
            LogMessage("=== SISTEMA LISTO PARA OPERACIONES ===");
        }

        private void CheckRequiredTools()
        {
            LogMessage("Verificando herramientas necesarias...");
            CheckAdbAvailability();
            CheckFastbootAvailability();
            LogMessage("Verificación de herramientas completada.");
        }

        private async void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "¿Estás seguro de que deseas cerrar sesión?",
                "Cerrar Sesión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (_currentSession != null)
                    {
                        await RemoteLicenseManager.Instance.LogoutAsync(_currentSession.Username, _currentSession.SessionToken);
                    }

                    SessionManager.ClearSession();
                    LicenseManager.Instance.ClearCurrentLicense();

                    LogMessage("Sesión cerrada correctamente.");
                    Application.Restart();
                }
                catch (Exception ex)
                {
                    LogError($"Error cerrando sesión: {ex.Message}");
                    SessionManager.ClearSession();
                    Application.Restart();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_licenseCheckTimer != null)
                {
                    _licenseCheckTimer.Stop();
                    _licenseCheckTimer.Dispose();
                }

                CloseComPort();

                if (httpClient != null)
                {
                    httpClient.Dispose();
                }

                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                    cancellationTokenSource.Dispose();
                }

                LogMessage("=== CIPHER UNLOCK PRO V1 CERRADO ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cerrando aplicación: {ex.Message}");
            }
        }

        #endregion
    }

    // Formulario avanzado para funciones adicionales
    public partial class AdvancedUnlockForm : Form
    {
        public AdvancedUnlockForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Funciones Avanzadas de Desbloqueo";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Implementar controles adicionales según necesidades
        }
    }
}