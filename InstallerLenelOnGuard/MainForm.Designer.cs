namespace InstallerLenelOnGuard
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btnInstallPath = new System.Windows.Forms.Button();
            this.btnInstall = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtDataSource = new System.Windows.Forms.TextBox();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblDBName = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPorAMSMobileAPI = new System.Windows.Forms.TextBox();
            this.txtPortAMSAPI = new System.Windows.Forms.TextBox();
            this.txtPortAMSWeb = new System.Windows.Forms.TextBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold);
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Location = new System.Drawing.Point(12, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(662, 42);
            this.label1.TabIndex = 0;
            this.label1.Text = "Database Setup";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnInstallPath
            // 
            this.btnInstallPath.AccessibleDescription = "Button to select the folder path in which the binaries will be installed";
            this.btnInstallPath.AccessibleName = "Select installation path";
            this.btnInstallPath.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnInstallPath.Font = new System.Drawing.Font("Arial", 14.25F);
            this.btnInstallPath.Location = new System.Drawing.Point(161, 165);
            this.btnInstallPath.Name = "btnInstallPath";
            this.btnInstallPath.Size = new System.Drawing.Size(321, 40);
            this.btnInstallPath.TabIndex = 1;
            this.btnInstallPath.Text = "Select installation path";
            this.btnInstallPath.UseVisualStyleBackColor = false;
            this.btnInstallPath.Click += new System.EventHandler(this.btnInstallPath_Click);
            // 
            // btnInstall
            // 
            this.btnInstall.AccessibleDescription = "Button to start installation of the services";
            this.btnInstall.AccessibleName = "Start installation";
            this.btnInstall.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnInstall.Enabled = false;
            this.btnInstall.Font = new System.Drawing.Font("Arial", 14.25F);
            this.btnInstall.Location = new System.Drawing.Point(488, 464);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(181, 40);
            this.btnInstall.TabIndex = 2;
            this.btnInstall.Text = "Start installation";
            this.btnInstall.UseVisualStyleBackColor = false;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // txtDataSource
            // 
            this.txtDataSource.Location = new System.Drawing.Point(161, 12);
            this.txtDataSource.Name = "txtDataSource";
            this.txtDataSource.Size = new System.Drawing.Size(321, 23);
            this.txtDataSource.TabIndex = 3;
            this.txtDataSource.Tag = "";
            this.txtDataSource.Text = "localhost\\\\SQLEXPRESS";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(161, 49);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(321, 23);
            this.txtDatabase.TabIndex = 4;
            this.txtDatabase.Text = "AMS";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(161, 120);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(321, 23);
            this.txtPassword.TabIndex = 5;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(161, 84);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(321, 23);
            this.txtUsername.TabIndex = 6;
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSource.Location = new System.Drawing.Point(6, 12);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(124, 20);
            this.lblSource.TabIndex = 7;
            this.lblSource.Text = "SQL Instance:";
            // 
            // lblDBName
            // 
            this.lblDBName.AutoSize = true;
            this.lblDBName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDBName.Location = new System.Drawing.Point(6, 49);
            this.lblDBName.Name = "lblDBName";
            this.lblDBName.Size = new System.Drawing.Size(141, 20);
            this.lblDBName.TabIndex = 8;
            this.lblDBName.Text = "Database name:";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.Location = new System.Drawing.Point(6, 84);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(133, 20);
            this.lblUserName.TabIndex = 9;
            this.lblUserName.Text = "SQL username:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.Location = new System.Drawing.Point(6, 120);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(130, 20);
            this.lblPassword.TabIndex = 10;
            this.lblPassword.Text = "SQL password:";
            // 
            // lblError
            // 
            this.lblError.CausesValidation = false;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.DarkRed;
            this.lblError.Location = new System.Drawing.Point(3, 307);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(643, 22);
            this.lblError.TabIndex = 12;
            this.lblError.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.AccessibleDescription = "Button to start installation of the services";
            this.btnTestConnection.AccessibleName = "Start installation";
            this.btnTestConnection.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnTestConnection.Font = new System.Drawing.Font("Arial", 14.25F);
            this.btnTestConnection.Location = new System.Drawing.Point(161, 223);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(321, 40);
            this.btnTestConnection.TabIndex = 4;
            this.btnTestConnection.Text = "Test connection";
            this.btnTestConnection.UseVisualStyleBackColor = false;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 15F);
            this.label2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label2.Location = new System.Drawing.Point(18, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(540, 27);
            this.label2.TabIndex = 13;
            this.label2.Text = "Site Configuration";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "Port AMS MobileAPI:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "Port AMS Web:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "Port AMS API:";
            // 
            // txtPorAMSMobileAPI
            // 
            this.txtPorAMSMobileAPI.Location = new System.Drawing.Point(183, 98);
            this.txtPorAMSMobileAPI.Name = "txtPorAMSMobileAPI";
            this.txtPorAMSMobileAPI.Size = new System.Drawing.Size(70, 23);
            this.txtPorAMSMobileAPI.TabIndex = 17;
            this.txtPorAMSMobileAPI.Tag = "";
            this.txtPorAMSMobileAPI.Text = "11000";
            // 
            // txtPortAMSAPI
            // 
            this.txtPortAMSAPI.Location = new System.Drawing.Point(183, 135);
            this.txtPortAMSAPI.Name = "txtPortAMSAPI";
            this.txtPortAMSAPI.Size = new System.Drawing.Size(70, 23);
            this.txtPortAMSAPI.TabIndex = 18;
            this.txtPortAMSAPI.Tag = "";
            this.txtPortAMSAPI.Text = "11001";
            // 
            // txtPortAMSWeb
            // 
            this.txtPortAMSWeb.Location = new System.Drawing.Point(183, 56);
            this.txtPortAMSWeb.Name = "txtPortAMSWeb";
            this.txtPortAMSWeb.Size = new System.Drawing.Size(70, 23);
            this.txtPortAMSWeb.TabIndex = 19;
            this.txtPortAMSWeb.Tag = "";
            this.txtPortAMSWeb.Text = "11002";
            // 
            // lblInfo
            // 
            this.lblInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblInfo.CausesValidation = false;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.lblInfo.Location = new System.Drawing.Point(3, 274);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(637, 22);
            this.lblInfo.TabIndex = 20;
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(18, 86);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(654, 372);
            this.tabControl1.TabIndex = 21;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblSource);
            this.tabPage1.Controls.Add(this.lblInfo);
            this.tabPage1.Controls.Add(this.txtDataSource);
            this.tabPage1.Controls.Add(this.lblDBName);
            this.tabPage1.Controls.Add(this.lblError);
            this.tabPage1.Controls.Add(this.txtDatabase);
            this.tabPage1.Controls.Add(this.lblUserName);
            this.tabPage1.Controls.Add(this.txtUsername);
            this.tabPage1.Controls.Add(this.lblPassword);
            this.tabPage1.Controls.Add(this.txtPassword);
            this.tabPage1.Controls.Add(this.btnInstallPath);
            this.tabPage1.Controls.Add(this.btnTestConnection);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(646, 343);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Configuration";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.txtPortAMSWeb);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.txtPorAMSMobileAPI);
            this.tabPage2.Controls.Add(this.txtPortAMSAPI);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(646, 343);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Advanced";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(19, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(650, 25);
            this.label6.TabIndex = 22;
            this.label6.Text = "Please make sure the empty database is already created";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 511);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnInstall);
            this.Name = "MainForm";
            this.Text = "AMSuite Installer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnInstallPath;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtDataSource;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblDBName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPorAMSMobileAPI;
        private System.Windows.Forms.TextBox txtPortAMSAPI;
        private System.Windows.Forms.TextBox txtPortAMSWeb;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label6;
    }
}

