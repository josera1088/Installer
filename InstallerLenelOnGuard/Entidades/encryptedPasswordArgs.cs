﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerLenelOnGuard.Entidades
{
    public class encryptedPasswordArgs : EventArgs
    {
        public string encryptedPassword { get; set; }
    }
}
