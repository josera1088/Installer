using InstallerLenelOnGuard.Entidades;
using InstallerLenelOnGuard.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace InstallerLenelOnGuard
{
    public partial class Status : Form
    {
        private string destination;
        private string DataSource;
        private string DatabaseName;
        private string UserName;
        private string Password;
        List<ModelPorts> Ports;
        Installer installer;
        MainForm main = new MainForm();
        public Status(string destinationPath, string dataSource, string databaseName, string username, string password, List<ModelPorts> ports)
        {

            destination = destinationPath;
            DataSource = dataSource;
            DatabaseName = databaseName;
            UserName = username;
            Password = password;
            Ports = ports;
            installer = new Installer(destination, DataSource, DatabaseName, UserName, Password, Ports);

            InitializeComponent();
#if DEBUG
            CheckForIllegalCrossThreadCalls = false;
#endif

            installer.enableButton += UIEnableButton;
            installer.PrintMessage += UIShowMessageTxtArea;
            installer.form = this;


            //Task.Run(() =>
            //{
                foreach (ModelPorts item in ports)
                {
                    if (item.IsService)
                        installer.InstallSc(item.SiteName, item.SiteName.Trim(), destinationPath);
                    else
                        installer.InstallAPI(item.SiteName, item.SitePort.ToString());
                }
        //    });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            main.Show();
        }

        public void UIShowMessageTxtArea(object sender, printMessageEventArgs e)
        {
            // Print the message on the textbox console.
            messagesConsole.Text += DateTime.Now.ToString("yyyy - MM - dd HH:mm:ss  : ") + e.module + "  " + e.message + Environment.NewLine;
            // Scroll to the end of the TextBox.
            messagesConsole.ScrollToCaret();
        }

        public void UIEnableButton(object sender, enableButtonArgs e)
        {
            switch (e.button)
            {
                case "close":
                    btnClose.Enabled = true;
                    break;
                    //case "back":
                    //    btnBack.Enabled = true;
                    //    break;
            };
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
    }
    public class printMessageEventArgs : EventArgs
    {
        public string message { get; set; }
        public string module { get; set; }
    }
}
