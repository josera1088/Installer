using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerLenelOnGuard.Entidades
{
    public class showLblsMessageArgs : EventArgs
    {
        public string message { get; set; } //what to write in the label
        public bool isError { get; set; } //is it error or information?
        public bool enablesInstallation { get; set; } //after this the install button should be accessible?
    }
}
