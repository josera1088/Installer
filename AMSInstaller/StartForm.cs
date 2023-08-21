using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AMSInstaller
{
    public partial class StartForm : Form
    {
        public System.Diagnostics.Process process = new System.Diagnostics.Process();
        public StartForm()
        {
            InitializeComponent();
#if DEBUG
            CheckForIllegalCrossThreadCalls = false;
#endif           

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            MainForm form = new MainForm();
            form.Show();
            this.Hide();
        }

        private void StartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try 
            {
                process = System.Diagnostics.Process.Start("msedge.exe", e.LinkText);
            }
            catch 
            {
                process = System.Diagnostics.Process.Start("chrome.exe", e.LinkText);
            }
        }
    }
}
