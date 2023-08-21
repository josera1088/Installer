using InstallerLenelOnGuard.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstallerLenelOnGuard
{
    public partial class MainForm : Form
    {


        string destination = "";
        string dataSource = "";
        string dBName = "";
        string dBUsername = "";
        string password = "";
        string encryptedPassword = "";
        ModelPorts modelPorts;
        List<ModelPorts> ListmodelPorts;
        Installer installer = new Installer();
        public delegate void UIChangeView();
        public UIChangeView changeViewDelegate;
        public MainForm()
        {

            InitializeComponent();
#if DEBUG
            CheckForIllegalCrossThreadCalls = false;
#endif
            installer.showLblsMessage += UIShowMessageLbls;
            installer.enableButton += UIEnableButton;
            installer.setEncryptedPassword += SetEncryptedPass;
            installer.form = this;
            changeViewDelegate = new UIChangeView(UIChangeViewMethod);
        }

        #region Private Methods

        private void btnInstallPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            destination = folderBrowserDialog1.SelectedPath;
            if (destination != null && destination != "")
            {
                Log.GetInstance().ChangeFullPath(destination);
                btnInstall.Enabled = true;
            }
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
           ListmodelPorts = new List<ModelPorts>();
            if (txtDataSource.Text == "" || txtDatabase.Text == "" || txtUsername.Text == "" || txtPassword.Text == "" || txtPortAMSAPI.Text == "" || txtPortAMSWeb.Text == "" || txtPorAMSMobileAPI.Text == "")
            {
                MessageBox.Show("Required fields are missing.");
            }
            else
            {
                btnInstall.Enabled = false;
                dataSource = txtDataSource.Text;
                dBName = txtDatabase.Text;
                dBUsername = txtUsername.Text;
                password = txtPassword.Text;
                modelPorts = new ModelPorts();
                modelPorts.SiteName = "AM LenelAPI";
                modelPorts.SitePort = Convert.ToInt16(txtPortAMSWeb.Text);
                modelPorts.IsService = false;
                ListmodelPorts.Add(modelPorts);

                modelPorts = new ModelPorts();
                modelPorts.SiteName = "AM LenelCommService";
                modelPorts.SitePort = Convert.ToInt16(txtPortAMSWeb.Text);
                modelPorts.IsService = true;
                ListmodelPorts.Add(modelPorts);


                lblError.Text = "";
                lblInfo.Text = "Testing connection...";
                var main = this;
                Task.Run(() => installer.ValidateInitConfig(dataSource, dBName, dBUsername, password, false, main, ListmodelPorts));
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {

            List<ModelPorts> ListmodelPorts = new List<ModelPorts>();

            dataSource = txtDataSource.Text;
            dBName = txtDatabase.Text;
            dBUsername = txtUsername.Text;
            password = txtPassword.Text;

            btnTestConnection.Enabled = false;
            lblError.Text = "";
            lblInfo.Text = "Testing connection...";
            var main = this;
            Task.Run(() => installer.ValidateInitConfig(dataSource, dBName, dBUsername, password, true, main, ListmodelPorts));
        }

        #endregion

        #region placeholders
        private void txtDataSource_TextEnter(object sender, EventArgs e)
        {
            if (txtDataSource.Text == "localhost\\SQLEXPRESS")
            {
                txtDataSource.Text = "";
            }
        }
        private void txtDataSource_TextLeave(object sender, EventArgs e)
        {
            if (txtDataSource.Text == "")
            {
                txtDataSource.Text = "localhost\\SQLEXPRESS";
            }
        }
        private void txtDatabase_TextEnter(object sender, EventArgs e)
        {
            if (txtDatabase.Text == "AMS")
            {
                txtDatabase.Text = "";
            }
        }
        private void txtDatabase_TextLeave(object sender, EventArgs e)
        {
            if (txtDatabase.Text == "")
            {
                txtDatabase.Text = "AMS";
            }
        }
        private void txtUsername_TextEnter(object sender, EventArgs e)
        {
            //if (txtUsername.Text == "alutel")
            //{
            //    txtUsername.Text = "";
            //}
        }
        private void txtUsername_TextLeave(object sender, EventArgs e)
        {
            //if (txtUsername.Text == "")
            //{
            //    txtUsername.Text = "alutel";
            //}
        }
        private void txtPassword_TextEnter(object sender, EventArgs e)
        {
            //if (txtPassword.Text == "alutel")
            //{
            //    txtPassword.Text = "";
            //}
        }
        private void txtPassword_TextLeave(object sender, EventArgs e)
        {
            //if (txtPassword.Text == "")
            //{
            //    txtPassword.Text = "alutel";
            //}
        }
        private void txtPortAMSWeb_Enter(object sender, EventArgs e)
        {
            if (txtPortAMSWeb.Text == "11002")
            {
                txtPortAMSWeb.Text = "";
            }
        }
        private void txtPortAMSWeb_Leave(object sender, EventArgs e)
        {
            if (txtPortAMSWeb.Text == "")
            {
                txtPortAMSWeb.Text = "11002";
            }
        }
        private void txtPortAMSAPI_Enter(object sender, EventArgs e)
        {
            if (txtPortAMSAPI.Text == "11001")
            {
                txtPortAMSAPI.Text = "";
            }
        }
        private void txtPortAMSAPI_Leave(object sender, EventArgs e)
        {
            if (txtPortAMSAPI.Text == "")
            {
                txtPortAMSAPI.Text = "11001";
            }
        }
        private void txtPorAMSMobileAPI_Enter(object sender, EventArgs e)
        {
            if (txtPorAMSMobileAPI.Text == "11000")
            {
                txtPorAMSMobileAPI.Text = "";
            }
        }
        private void txtPorAMSMobileAPI_Leave(object sender, EventArgs e)
        {
            if (txtPorAMSMobileAPI.Text == "")
            {
                txtPorAMSMobileAPI.Text = "11000";
            }
        }


        #endregion placeholders

        #region Public Methods 

        public void UIChangeViewMethod()
        {
            try
            {
                this.Hide();
                Status statusInstallation = new Status(destination, dataSource, dBName, dBUsername, encryptedPassword, ListmodelPorts);
                statusInstallation.Show();
                statusInstallation.Closed += (s, args) => this.Close();
            }
            catch (Exception ex)
            {
                Log.GetInstance().DoLog("ChangeView failed to change from main form to status form: ", "General", ex);
                lblInfo.Text = "";
                lblError.Text = "Error opening next from";
                this.Show();
            }
        }

        public void UIShowMessageLbls(object sender, showLblsMessageArgs e)
        {
            if (e.isError)
            {
                lblInfo.Text = "";
                lblError.Text = e.message;
            }
            else
            {
                lblError.Text = "";
                lblInfo.Text = e.message;
            }
        }

        public void UIEnableButton(object sender, enableButtonArgs e)
        {
            switch (e.button)
            {
                case "connection":
                    btnTestConnection.Enabled = true;
                    break;
                case "install":
                    btnInstall.Enabled = true;
                    break;
            };
        }

        public void SetEncryptedPass(object sender, encryptedPasswordArgs e)
        {
            encryptedPassword = e.encryptedPassword;
        }

        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
