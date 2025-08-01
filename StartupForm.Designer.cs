namespace CipherUnlockProV1
{
    partial class StartupForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.LinkLabel lnkRegister;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panelLicense;
        private System.Windows.Forms.Label lblLicenseInfo;
        private System.Windows.Forms.TextBox txtLicenseKey;
        private System.Windows.Forms.Button btnActivateLicense;
        private System.Windows.Forms.Button btnPurchaseLicense;
        private System.Windows.Forms.RadioButton radioBtn3Month;
        private System.Windows.Forms.RadioButton radioBtn6Month;
        private System.Windows.Forms.RadioButton radioBtn12Month;
        private System.Windows.Forms.GroupBox groupBoxLicenseType;

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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lnkRegister = new System.Windows.Forms.LinkLabel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelLicense = new System.Windows.Forms.Panel();
            this.lblLicenseInfo = new System.Windows.Forms.Label();
            this.txtLicenseKey = new System.Windows.Forms.TextBox();
            this.btnActivateLicense = new System.Windows.Forms.Button();
            this.btnPurchaseLicense = new System.Windows.Forms.Button();
            this.groupBoxLicenseType = new System.Windows.Forms.GroupBox();
            this.radioBtn3Month = new System.Windows.Forms.RadioButton();
            this.radioBtn6Month = new System.Windows.Forms.RadioButton();
            this.radioBtn12Month = new System.Windows.Forms.RadioButton();
            this.panelLicense.SuspendLayout();
            this.groupBoxLicenseType.SuspendLayout();
            this.SuspendLayout();

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(50, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(350, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Iniciar Sesión - CipherUnlock Pro V1";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUsername.ForeColor = System.Drawing.Color.White;
            this.lblUsername.Location = new System.Drawing.Point(30, 80);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(118, 19);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Nombre de Usuario:";

            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtUsername.Location = new System.Drawing.Point(30, 105);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(380, 25);
            this.txtUsername.TabIndex = 2;

            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPassword.ForeColor = System.Drawing.Color.White;
            this.lblPassword.Location = new System.Drawing.Point(30, 140);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(73, 19);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Contraseña:";

            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPassword.Location = new System.Drawing.Point(30, 165);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(380, 25);
            this.txtPassword.TabIndex = 4;

            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmail.ForeColor = System.Drawing.Color.White;
            this.lblEmail.Location = new System.Drawing.Point(30, 200);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(119, 19);
            this.lblEmail.TabIndex = 5;
            this.lblEmail.Text = "Correo Electrónico:";
            this.lblEmail.Visible = false;

            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtEmail.Location = new System.Drawing.Point(30, 225);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(380, 25);
            this.txtEmail.TabIndex = 6;
            this.txtEmail.Visible = false;

            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblConfirmPassword.ForeColor = System.Drawing.Color.White;
            this.lblConfirmPassword.Location = new System.Drawing.Point(30, 260);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(138, 19);
            this.lblConfirmPassword.TabIndex = 7;
            this.lblConfirmPassword.Text = "Confirmar Contraseña:";
            this.lblConfirmPassword.Visible = false;

            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtConfirmPassword.Location = new System.Drawing.Point(30, 285);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '*';
            this.txtConfirmPassword.Size = new System.Drawing.Size(380, 25);
            this.txtConfirmPassword.TabIndex = 8;
            this.txtConfirmPassword.Visible = false;

            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(30, 320);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(380, 40);
            this.btnLogin.TabIndex = 9;
            this.btnLogin.Text = "Iniciar Sesión";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // 
            // lnkRegister
            // 
            this.lnkRegister.AutoSize = true;
            this.lnkRegister.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkRegister.LinkColor = System.Drawing.Color.LightBlue;
            this.lnkRegister.Location = new System.Drawing.Point(125, 375);
            this.lnkRegister.Name = "lnkRegister";
            this.lnkRegister.Size = new System.Drawing.Size(200, 15);
            this.lnkRegister.TabIndex = 10;
            this.lnkRegister.TabStop = true;
            this.lnkRegister.Text = "¿No tienes cuenta? Regístrate aquí";
            this.lnkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRegister_LinkClicked);

            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.Yellow;
            this.lblStatus.Location = new System.Drawing.Point(30, 405);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 15);
            this.lblStatus.TabIndex = 11;

            // 
            // panelLicense
            // 
            this.panelLicense.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.panelLicense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLicense.Controls.Add(this.lblLicenseInfo);
            this.panelLicense.Controls.Add(this.txtLicenseKey);
            this.panelLicense.Controls.Add(this.btnActivateLicense);
            this.panelLicense.Controls.Add(this.btnPurchaseLicense);
            this.panelLicense.Controls.Add(this.groupBoxLicenseType);
            this.panelLicense.Location = new System.Drawing.Point(30, 430);
            this.panelLicense.Name = "panelLicense";
            this.panelLicense.Size = new System.Drawing.Size(380, 280);
            this.panelLicense.TabIndex = 12;
            this.panelLicense.Visible = false;

            // 
            // lblLicenseInfo
            // 
            this.lblLicenseInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLicenseInfo.ForeColor = System.Drawing.Color.White;
            this.lblLicenseInfo.Location = new System.Drawing.Point(10, 10);
            this.lblLicenseInfo.Name = "lblLicenseInfo";
            this.lblLicenseInfo.Size = new System.Drawing.Size(355, 50);
            this.lblLicenseInfo.TabIndex = 0;
            this.lblLicenseInfo.Text = "No se encontró licencia activa. Por favor activa una clave de licencia o compra una nueva licencia.";

            // 
            // txtLicenseKey
            // 
            this.txtLicenseKey.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtLicenseKey.Location = new System.Drawing.Point(10, 70);
            this.txtLicenseKey.Name = "txtLicenseKey";
            this.txtLicenseKey.Size = new System.Drawing.Size(355, 25);
            this.txtLicenseKey.TabIndex = 1;

            // 
            // btnActivateLicense
            // 
            this.btnActivateLicense.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(0)))));
            this.btnActivateLicense.FlatAppearance.BorderSize = 0;
            this.btnActivateLicense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivateLicense.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnActivateLicense.ForeColor = System.Drawing.Color.White;
            this.btnActivateLicense.Location = new System.Drawing.Point(10, 105);
            this.btnActivateLicense.Name = "btnActivateLicense";
            this.btnActivateLicense.Size = new System.Drawing.Size(355, 35);
            this.btnActivateLicense.TabIndex = 2;
            this.btnActivateLicense.Text = "Activar Licencia";
            this.btnActivateLicense.UseVisualStyleBackColor = false;
            this.btnActivateLicense.Click += new System.EventHandler(this.btnActivateLicense_Click);

            // 
            // groupBoxLicenseType
            // 
            this.groupBoxLicenseType.Controls.Add(this.radioBtn3Month);
            this.groupBoxLicenseType.Controls.Add(this.radioBtn6Month);
            this.groupBoxLicenseType.Controls.Add(this.radioBtn12Month);
            this.groupBoxLicenseType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBoxLicenseType.ForeColor = System.Drawing.Color.White;
            this.groupBoxLicenseType.Location = new System.Drawing.Point(10, 150);
            this.groupBoxLicenseType.Name = "groupBoxLicenseType";
            this.groupBoxLicenseType.Size = new System.Drawing.Size(355, 80);
            this.groupBoxLicenseType.TabIndex = 3;
            this.groupBoxLicenseType.TabStop = false;
            this.groupBoxLicenseType.Text = "Tipo de Licencia";

            // 
            // radioBtn3Month
            // 
            this.radioBtn3Month.AutoSize = true;
            this.radioBtn3Month.Checked = true;
            this.radioBtn3Month.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioBtn3Month.ForeColor = System.Drawing.Color.White;
            this.radioBtn3Month.Location = new System.Drawing.Point(15, 25);
            this.radioBtn3Month.Name = "radioBtn3Month";
            this.radioBtn3Month.Size = new System.Drawing.Size(78, 19);
            this.radioBtn3Month.TabIndex = 0;
            this.radioBtn3Month.TabStop = true;
            this.radioBtn3Month.Text = "3 Meses";
            this.radioBtn3Month.UseVisualStyleBackColor = true;

            // 
            // radioBtn6Month
            // 
            this.radioBtn6Month.AutoSize = true;
            this.radioBtn6Month.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioBtn6Month.ForeColor = System.Drawing.Color.White;
            this.radioBtn6Month.Location = new System.Drawing.Point(125, 25);
            this.radioBtn6Month.Name = "radioBtn6Month";
            this.radioBtn6Month.Size = new System.Drawing.Size(78, 19);
            this.radioBtn6Month.TabIndex = 1;
            this.radioBtn6Month.Text = "6 Meses";
            this.radioBtn6Month.UseVisualStyleBackColor = true;

            // 
            // radioBtn12Month
            // 
            this.radioBtn12Month.AutoSize = true;
            this.radioBtn12Month.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radioBtn12Month.ForeColor = System.Drawing.Color.White;
            this.radioBtn12Month.Location = new System.Drawing.Point(235, 25);
            this.radioBtn12Month.Name = "radioBtn12Month";
            this.radioBtn12Month.Size = new System.Drawing.Size(82, 19);
            this.radioBtn12Month.TabIndex = 2;
            this.radioBtn12Month.Text = "12 Meses";
            this.radioBtn12Month.UseVisualStyleBackColor = true;

            // 
            // btnPurchaseLicense
            // 
            this.btnPurchaseLicense.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(0)))));
            this.btnPurchaseLicense.FlatAppearance.BorderSize = 0;
            this.btnPurchaseLicense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPurchaseLicense.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnPurchaseLicense.ForeColor = System.Drawing.Color.White;
            this.btnPurchaseLicense.Location = new System.Drawing.Point(10, 240);
            this.btnPurchaseLicense.Name = "btnPurchaseLicense";
            this.btnPurchaseLicense.Size = new System.Drawing.Size(355, 35);
            this.btnPurchaseLicense.TabIndex = 4;
            this.btnPurchaseLicense.Text = "Comprar Licencia";
            this.btnPurchaseLicense.UseVisualStyleBackColor = false;
            this.btnPurchaseLicense.Click += new System.EventHandler(this.btnPurchaseLicense_Click);

            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(450, 750);
            this.Controls.Add(this.panelLicense);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lnkRegister);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CipherUnlock Pro V1 - Autenticación";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartupForm_FormClosing);
            this.panelLicense.ResumeLayout(false);
            this.panelLicense.PerformLayout();
            this.groupBoxLicenseType.ResumeLayout(false);
            this.groupBoxLicenseType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}