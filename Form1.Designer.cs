using CipherUnlockProV1.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace CipherUnlockProV1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Controles de información de licencia
        private System.Windows.Forms.Panel panelLicense;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblLicenseType;
        private System.Windows.Forms.Label lblLicenseStatus;
        private System.Windows.Forms.Label lblExpiryDate;
        private System.Windows.Forms.Label lblDaysRemaining;
        private System.Windows.Forms.Button btnLogout;

        // Controles principales
        private System.Windows.Forms.TabControl tabControlUnlock;
        private System.Windows.Forms.TabPage tabAdbUnlock;
        private System.Windows.Forms.TabPage tabQualcommUnlock;
        private System.Windows.Forms.TabPage tabAdvancedUnlock;
        private System.Windows.Forms.TabPage tabLogs;

        // Controles para dispositivos ADB
        private System.Windows.Forms.ComboBox cmbDevices;
        private System.Windows.Forms.Button btnRefreshDevices;
        private System.Windows.Forms.Label lblDeviceCount;
        private System.Windows.Forms.Label lblDeviceInfo;
        private System.Windows.Forms.Button btnUnlockDevice;

        // Controles para puertos COM
        private System.Windows.Forms.ComboBox cmbComPorts;
        private System.Windows.Forms.Button btnRefreshComPorts;
        private System.Windows.Forms.Label lblComPortCount;
        private System.Windows.Forms.Button btnQualcommUnlock;

        // Controles de estado y progreso
        private System.Windows.Forms.ProgressBar progressBarUnlock;
        private System.Windows.Forms.Label lblStatusMessage;
        private System.Windows.Forms.Button btnCancelOperation;

        // Controles de logging
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnSaveLog;

        // Controles de estado de herramientas
        private System.Windows.Forms.Label lblAdbStatus;
        private System.Windows.Forms.Label lblFastbootStatus;

        // Controles avanzados
        private System.Windows.Forms.Button btnAdvancedUnlock;
        private System.Windows.Forms.TextBox txtCustomCommand;
        private System.Windows.Forms.Button btnExecuteCommand;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.panelLicense = new System.Windows.Forms.Panel();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblLicenseType = new System.Windows.Forms.Label();
            this.lblLicenseStatus = new System.Windows.Forms.Label();
            this.lblExpiryDate = new System.Windows.Forms.Label();
            this.lblDaysRemaining = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.tabControlUnlock = new System.Windows.Forms.TabControl();
            this.tabAdbUnlock = new System.Windows.Forms.TabPage();
            this.tabQualcommUnlock = new System.Windows.Forms.TabPage();
            this.tabAdvancedUnlock = new System.Windows.Forms.TabPage();
            this.tabLogs = new System.Windows.Forms.TabPage();
            this.cmbDevices = new System.Windows.Forms.ComboBox();
            this.btnRefreshDevices = new System.Windows.Forms.Button();
            this.lblDeviceCount = new System.Windows.Forms.Label();
            this.lblDeviceInfo = new System.Windows.Forms.Label();
            this.btnUnlockDevice = new System.Windows.Forms.Button();
            this.cmbComPorts = new System.Windows.Forms.ComboBox();
            this.btnRefreshComPorts = new System.Windows.Forms.Button();
            this.lblComPortCount = new System.Windows.Forms.Label();
            this.btnQualcommUnlock = new System.Windows.Forms.Button();
            this.progressBarUnlock = new System.Windows.Forms.ProgressBar();
            this.lblStatusMessage = new System.Windows.Forms.Label();
            this.btnCancelOperation = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnSaveLog = new System.Windows.Forms.Button();
            this.lblAdbStatus = new System.Windows.Forms.Label();
            this.lblFastbootStatus = new System.Windows.Forms.Label();
            this.btnAdvancedUnlock = new System.Windows.Forms.Button();
            this.txtCustomCommand = new System.Windows.Forms.TextBox();
            this.btnExecuteCommand = new System.Windows.Forms.Button();
            this.panelLicense.SuspendLayout();
            this.tabControlUnlock.SuspendLayout();
            this.tabAdbUnlock.SuspendLayout();
            this.tabQualcommUnlock.SuspendLayout();
            this.tabAdvancedUnlock.SuspendLayout();
            this.tabLogs.SuspendLayout();
            this.SuspendLayout();

            // 
            // panelLicense
            // 
            this.panelLicense.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.panelLicense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLicense.Controls.Add(this.lblUsername);
            this.panelLicense.Controls.Add(this.lblLicenseType);
            this.panelLicense.Controls.Add(this.lblLicenseStatus);
            this.panelLicense.Controls.Add(this.lblExpiryDate);
            this.panelLicense.Controls.Add(this.lblDaysRemaining);
            this.panelLicense.Controls.Add(this.btnLogout);
            this.panelLicense.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLicense.Location = new System.Drawing.Point(0, 0);
            this.panelLicense.Name = "panelLicense";
            this.panelLicense.Size = new System.Drawing.Size(1200, 80);
            this.panelLicense.TabIndex = 0;

            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.White;
            this.lblUsername.Location = new System.Drawing.Point(15, 15);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(150, 20);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Usuario: Cargando...";

            // 
            // lblLicenseType
            // 
            this.lblLicenseType.AutoSize = true;
            this.lblLicenseType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLicenseType.ForeColor = System.Drawing.Color.White;
            this.lblLicenseType.Location = new System.Drawing.Point(250, 15);
            this.lblLicenseType.Name = "lblLicenseType";
            this.lblLicenseType.Size = new System.Drawing.Size(120, 19);
            this.lblLicenseType.TabIndex = 1;
            this.lblLicenseType.Text = "Licencia: Cargando...";

            // 
            // lblLicenseStatus
            // 
            this.lblLicenseStatus.AutoSize = true;
            this.lblLicenseStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLicenseStatus.ForeColor = System.Drawing.Color.White;
            this.lblLicenseStatus.Location = new System.Drawing.Point(450, 15);
            this.lblLicenseStatus.Name = "lblLicenseStatus";
            this.lblLicenseStatus.Size = new System.Drawing.Size(110, 19);
            this.lblLicenseStatus.TabIndex = 2;
            this.lblLicenseStatus.Text = "Estado: Cargando...";

            // 
            // lblExpiryDate
            // 
            this.lblExpiryDate.AutoSize = true;
            this.lblExpiryDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblExpiryDate.ForeColor = System.Drawing.Color.White;
            this.lblExpiryDate.Location = new System.Drawing.Point(15, 45);
            this.lblExpiryDate.Name = "lblExpiryDate";
            this.lblExpiryDate.Size = new System.Drawing.Size(100, 19);
            this.lblExpiryDate.TabIndex = 3;
            this.lblExpiryDate.Text = "Vence: Cargando...";

            // 
            // lblDaysRemaining
            // 
            this.lblDaysRemaining.AutoSize = true;
            this.lblDaysRemaining.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDaysRemaining.ForeColor = System.Drawing.Color.Green;
            this.lblDaysRemaining.Location = new System.Drawing.Point(250, 45);
            this.lblDaysRemaining.Name = "lblDaysRemaining";
            this.lblDaysRemaining.Size = new System.Drawing.Size(150, 19);
            this.lblDaysRemaining.TabIndex = 4;
            this.lblDaysRemaining.Text = "Días restantes: Cargando...";

            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(1080, 25);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 30);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Cerrar Sesión";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // 
            // tabControlUnlock
            // 
            this.tabControlUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlUnlock.Controls.Add(this.tabAdbUnlock);
            this.tabControlUnlock.Controls.Add(this.tabQualcommUnlock);
            this.tabControlUnlock.Controls.Add(this.tabAdvancedUnlock);
            this.tabControlUnlock.Controls.Add(this.tabLogs);
            this.tabControlUnlock.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControlUnlock.Location = new System.Drawing.Point(12, 95);
            this.tabControlUnlock.Name = "tabControlUnlock";
            this.tabControlUnlock.SelectedIndex = 0;
            this.tabControlUnlock.Size = new System.Drawing.Size(1176, 550);
            this.tabControlUnlock.TabIndex = 1;

            // 
            // tabAdbUnlock
            // 
            this.tabAdbUnlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabAdbUnlock.Controls.Add(this.cmbDevices);
            this.tabAdbUnlock.Controls.Add(this.btnRefreshDevices);
            this.tabAdbUnlock.Controls.Add(this.lblDeviceCount);
            this.tabAdbUnlock.Controls.Add(this.lblDeviceInfo);
            this.tabAdbUnlock.Controls.Add(this.btnUnlockDevice);
            this.tabAdbUnlock.Controls.Add(this.lblAdbStatus);
            this.tabAdbUnlock.Location = new System.Drawing.Point(4, 28);
            this.tabAdbUnlock.Name = "tabAdbUnlock";
            this.tabAdbUnlock.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdbUnlock.Size = new System.Drawing.Size(1168, 518);
            this.tabAdbUnlock.TabIndex = 0;
            this.tabAdbUnlock.Text = "Desbloqueo ADB";

            // 
            // cmbDevices
            // 
            this.cmbDevices.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.cmbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDevices.ForeColor = System.Drawing.Color.White;
            this.cmbDevices.FormattingEnabled = true;
            this.cmbDevices.Location = new System.Drawing.Point(25, 80);
            this.cmbDevices.Name = "cmbDevices";
            this.cmbDevices.Size = new System.Drawing.Size(400, 25);
            this.cmbDevices.TabIndex = 0;

            // 
            // btnRefreshDevices
            // 
            this.btnRefreshDevices.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnRefreshDevices.FlatAppearance.BorderSize = 0;
            this.btnRefreshDevices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshDevices.ForeColor = System.Drawing.Color.White;
            this.btnRefreshDevices.Location = new System.Drawing.Point(450, 80);
            this.btnRefreshDevices.Name = "btnRefreshDevices";
            this.btnRefreshDevices.Size = new System.Drawing.Size(120, 25);
            this.btnRefreshDevices.TabIndex = 1;
            this.btnRefreshDevices.Text = "Actualizar";
            this.btnRefreshDevices.UseVisualStyleBackColor = false;

            // 
            // lblDeviceCount
            // 
            this.lblDeviceCount.AutoSize = true;
            this.lblDeviceCount.ForeColor = System.Drawing.Color.White;
            this.lblDeviceCount.Location = new System.Drawing.Point(25, 125);
            this.lblDeviceCount.Name = "lblDeviceCount";
            this.lblDeviceCount.Size = new System.Drawing.Size(150, 19);
            this.lblDeviceCount.TabIndex = 2;
            this.lblDeviceCount.Text = "Dispositivos encontrados: 0";

            // 
            // lblDeviceInfo
            // 
            this.lblDeviceInfo.AutoSize = true;
            this.lblDeviceInfo.ForeColor = System.Drawing.Color.White;
            this.lblDeviceInfo.Location = new System.Drawing.Point(25, 155);
            this.lblDeviceInfo.Name = "lblDeviceInfo";
            this.lblDeviceInfo.Size = new System.Drawing.Size(200, 19);
            this.lblDeviceInfo.TabIndex = 3;
            this.lblDeviceInfo.Text = "Información del dispositivo: N/A";

            // 
            // btnUnlockDevice
            // 
            this.btnUnlockDevice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnUnlockDevice.FlatAppearance.BorderSize = 0;
            this.btnUnlockDevice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnlockDevice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnUnlockDevice.ForeColor = System.Drawing.Color.White;
            this.btnUnlockDevice.Location = new System.Drawing.Point(25, 200);
            this.btnUnlockDevice.Name = "btnUnlockDevice";
            this.btnUnlockDevice.Size = new System.Drawing.Size(300, 50);
            this.btnUnlockDevice.TabIndex = 4;
            this.btnUnlockDevice.Text = "Desbloquear Dispositivo";
            this.btnUnlockDevice.UseVisualStyleBackColor = false;

            // 
            // lblAdbStatus
            // 
            this.lblAdbStatus.AutoSize = true;
            this.lblAdbStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAdbStatus.ForeColor = System.Drawing.Color.Yellow;
            this.lblAdbStatus.Location = new System.Drawing.Point(25, 30);
            this.lblAdbStatus.Name = "lblAdbStatus";
            this.lblAdbStatus.Size = new System.Drawing.Size(150, 19);
            this.lblAdbStatus.TabIndex = 5;
            this.lblAdbStatus.Text = "Estado ADB: Verificando...";

            // 
            // tabQualcommUnlock
            // 
            this.tabQualcommUnlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabQualcommUnlock.Controls.Add(this.cmbComPorts);
            this.tabQualcommUnlock.Controls.Add(this.btnRefreshComPorts);
            this.tabQualcommUnlock.Controls.Add(this.lblComPortCount);
            this.tabQualcommUnlock.Controls.Add(this.btnQualcommUnlock);
            this.tabQualcommUnlock.Location = new System.Drawing.Point(4, 28);
            this.tabQualcommUnlock.Name = "tabQualcommUnlock";
            this.tabQualcommUnlock.Padding = new System.Windows.Forms.Padding(3);
            this.tabQualcommUnlock.Size = new System.Drawing.Size(1168, 518);
            this.tabQualcommUnlock.TabIndex = 1;
            this.tabQualcommUnlock.Text = "Desbloqueo Qualcomm";

            // 
            // cmbComPorts
            // 
            this.cmbComPorts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.cmbComPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbComPorts.ForeColor = System.Drawing.Color.White;
            this.cmbComPorts.FormattingEnabled = true;
            this.cmbComPorts.Location = new System.Drawing.Point(25, 80);
            this.cmbComPorts.Name = "cmbComPorts";
            this.cmbComPorts.Size = new System.Drawing.Size(200, 25);
            this.cmbComPorts.TabIndex = 0;

            // 
            // btnRefreshComPorts
            // 
            this.btnRefreshComPorts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnRefreshComPorts.FlatAppearance.BorderSize = 0;
            this.btnRefreshComPorts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshComPorts.ForeColor = System.Drawing.Color.White;
            this.btnRefreshComPorts.Location = new System.Drawing.Point(250, 80);
            this.btnRefreshComPorts.Name = "btnRefreshComPorts";
            this.btnRefreshComPorts.Size = new System.Drawing.Size(120, 25);
            this.btnRefreshComPorts.TabIndex = 1;
            this.btnRefreshComPorts.Text = "Actualizar";
            this.btnRefreshComPorts.UseVisualStyleBackColor = false;

            // 
            // lblComPortCount
            // 
            this.lblComPortCount.AutoSize = true;
            this.lblComPortCount.ForeColor = System.Drawing.Color.White;
            this.lblComPortCount.Location = new System.Drawing.Point(25, 125);
            this.lblComPortCount.Name = "lblComPortCount";
            this.lblComPortCount.Size = new System.Drawing.Size(180, 19);
            this.lblComPortCount.TabIndex = 2;
            this.lblComPortCount.Text = "Puertos COM encontrados: 0";

            // 
            // btnQualcommUnlock
            // 
            this.btnQualcommUnlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))));
            this.btnQualcommUnlock.FlatAppearance.BorderSize = 0;
            this.btnQualcommUnlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQualcommUnlock.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnQualcommUnlock.ForeColor = System.Drawing.Color.White;
            this.btnQualcommUnlock.Location = new System.Drawing.Point(25, 170);
            this.btnQualcommUnlock.Name = "btnQualcommUnlock";
            this.btnQualcommUnlock.Size = new System.Drawing.Size(300, 50);
            this.btnQualcommUnlock.TabIndex = 3;
            this.btnQualcommUnlock.Text = "Desbloquear Qualcomm";
            this.btnQualcommUnlock.UseVisualStyleBackColor = false;

            // 
            // tabAdvancedUnlock
            // 
            this.tabAdvancedUnlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabAdvancedUnlock.Controls.Add(this.btnAdvancedUnlock);
            this.tabAdvancedUnlock.Controls.Add(this.txtCustomCommand);
            this.tabAdvancedUnlock.Controls.Add(this.btnExecuteCommand);
            this.tabAdvancedUnlock.Controls.Add(this.lblFastbootStatus);
            this.tabAdvancedUnlock.Location = new System.Drawing.Point(4, 28);
            this.tabAdvancedUnlock.Name = "tabAdvancedUnlock";
            this.tabAdvancedUnlock.Size = new System.Drawing.Size(1168, 518);
            this.tabAdvancedUnlock.TabIndex = 2;
            this.tabAdvancedUnlock.Text = "Funciones Avanzadas";

            // 
            // btnAdvancedUnlock
            // 
            this.btnAdvancedUnlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnAdvancedUnlock.FlatAppearance.BorderSize = 0;
            this.btnAdvancedUnlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdvancedUnlock.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnAdvancedUnlock.ForeColor = System.Drawing.Color.White;
            this.btnAdvancedUnlock.Location = new System.Drawing.Point(25, 80);
            this.btnAdvancedUnlock.Name = "btnAdvancedUnlock";
            this.btnAdvancedUnlock.Size = new System.Drawing.Size(300, 50);
            this.btnAdvancedUnlock.TabIndex = 0;
            this.btnAdvancedUnlock.Text = "Desbloqueo Avanzado";
            this.btnAdvancedUnlock.UseVisualStyleBackColor = false;

            // 
            // txtCustomCommand
            // 
            this.txtCustomCommand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtCustomCommand.ForeColor = System.Drawing.Color.White;
            this.txtCustomCommand.Location = new System.Drawing.Point(25, 180);
            this.txtCustomCommand.Name = "txtCustomCommand";
            this.txtCustomCommand.Size = new System.Drawing.Size(500, 25);
            this.txtCustomCommand.TabIndex = 1;

            // 
            // btnExecuteCommand
            // 
            this.btnExecuteCommand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnExecuteCommand.FlatAppearance.BorderSize = 0;
            this.btnExecuteCommand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExecuteCommand.ForeColor = System.Drawing.Color.White;
            this.btnExecuteCommand.Location = new System.Drawing.Point(550, 180);
            this.btnExecuteCommand.Name = "btnExecuteCommand";
            this.btnExecuteCommand.Size = new System.Drawing.Size(120, 25);
            this.btnExecuteCommand.TabIndex = 2;
            this.btnExecuteCommand.Text = "Ejecutar";
            this.btnExecuteCommand.UseVisualStyleBackColor = false;

            // 
            // lblFastbootStatus
            // 
            this.lblFastbootStatus.AutoSize = true;
            this.lblFastbootStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFastbootStatus.ForeColor = System.Drawing.Color.Yellow;
            this.lblFastbootStatus.Location = new System.Drawing.Point(25, 30);
            this.lblFastbootStatus.Name = "lblFastbootStatus";
            this.lblFastbootStatus.Size = new System.Drawing.Size(180, 19);
            this.lblFastbootStatus.TabIndex = 3;
            this.lblFastbootStatus.Text = "Estado Fastboot: Verificando...";

            // 
            // tabLogs
            // 
            this.tabLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabLogs.Controls.Add(this.txtLog);
            this.tabLogs.Controls.Add(this.btnClearLog);
            this.tabLogs.Controls.Add(this.btnSaveLog);
            this.tabLogs.Location = new System.Drawing.Point(4, 28);
            this.tabLogs.Name = "tabLogs";
            this.tabLogs.Size = new System.Drawing.Size(1168, 518);
            this.tabLogs.TabIndex = 3;
            this.tabLogs.Text = "Registro de Actividad";

            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.ForeColor = System.Drawing.Color.Lime;
            this.txtLog.Location = new System.Drawing.Point(15, 50);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(1138, 450);
            this.txtLog.TabIndex = 0;

            // 
            // btnClearLog
            // 
            this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnClearLog.FlatAppearance.BorderSize = 0;
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLog.ForeColor = System.Drawing.Color.White;
            this.btnClearLog.Location = new System.Drawing.Point(15, 15);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(100, 30);
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.Text = "Limpiar";
            this.btnClearLog.UseVisualStyleBackColor = false;

            // 
            // btnSaveLog
            // 
            this.btnSaveLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnSaveLog.FlatAppearance.BorderSize = 0;
            this.btnSaveLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveLog.ForeColor = System.Drawing.Color.White;
            this.btnSaveLog.Location = new System.Drawing.Point(130, 15);
            this.btnSaveLog.Name = "btnSaveLog";
            this.btnSaveLog.Size = new System.Drawing.Size(100, 30);
            this.btnSaveLog.TabIndex = 2;
            this.btnSaveLog.Text = "Guardar";
            this.btnSaveLog.UseVisualStyleBackColor = false;

            // 
            // progressBarUnlock
            // 
            this.progressBarUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarUnlock.Location = new System.Drawing.Point(12, 660);
            this.progressBarUnlock.Name = "progressBarUnlock";
            this.progressBarUnlock.Size = new System.Drawing.Size(800, 25);
            this.progressBarUnlock.TabIndex = 2;

            // 
            // lblStatusMessage
            // 
            this.lblStatusMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatusMessage.AutoSize = true;
            this.lblStatusMessage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatusMessage.ForeColor = System.Drawing.Color.White;
            this.lblStatusMessage.Location = new System.Drawing.Point(12, 695);
            this.lblStatusMessage.Name = "lblStatusMessage";
            this.lblStatusMessage.Size = new System.Drawing.Size(150, 19);
            this.lblStatusMessage.TabIndex = 3;
            this.lblStatusMessage.Text = "Listo para operaciones";

            // 
            // btnCancelOperation
            // 
            this.btnCancelOperation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelOperation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCancelOperation.Enabled = false;
            this.btnCancelOperation.FlatAppearance.BorderSize = 0;
            this.btnCancelOperation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelOperation.ForeColor = System.Drawing.Color.White;
            this.btnCancelOperation.Location = new System.Drawing.Point(1070, 660);
            this.btnCancelOperation.Name = "btnCancelOperation";
            this.btnCancelOperation.Size = new System.Drawing.Size(118, 25);
            this.btnCancelOperation.TabIndex = 4;
            this.btnCancelOperation.Text = "Cancelar";
            this.btnCancelOperation.UseVisualStyleBackColor = false;

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(1200, 730);
            this.Controls.Add(this.btnCancelOperation);
            this.Controls.Add(this.lblStatusMessage);
            this.Controls.Add(this.progressBarUnlock);
            this.Controls.Add(this.tabControlUnlock);
            this.Controls.Add(this.panelLicense);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1200, 730);
            this.Name = "Form1";
            this.Text = "CipherUnlock Pro V1 - Herramienta de Desbloqueo de Dispositivos";
            this.WindowState = FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelLicense.ResumeLayout(false);
            this.panelLicense.PerformLayout();
            this.tabControlUnlock.ResumeLayout(false);
            this.tabAdbUnlock.ResumeLayout(false);
            this.tabAdbUnlock.PerformLayout();
            this.tabQualcommUnlock.ResumeLayout(false);
            this.tabQualcommUnlock.PerformLayout();
            this.tabAdvancedUnlock.ResumeLayout(false);
            this.tabAdvancedUnlock.PerformLayout();
            this.tabLogs.ResumeLayout(false);
            this.tabLogs.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}