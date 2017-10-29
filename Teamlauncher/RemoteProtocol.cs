﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teamlauncher
{
    public abstract class RemoteProtocol
    {
        public int id;
        public string name;
        public int defaultPort;
        public Bitmap icon;

        public const int ParamNone = 0x00;
        public const int ParamLogin = 0x01;
        public const int ParamPassword = 0x02;
        public const int ParamHost = 0x04;
        public const int ParamPort = 0x08;

        public RemoteProtocol()
        {
            name = "unamed";
            defaultPort = 0;
        }

        public override string ToString()
        {
            return name;
        }

        public abstract void run(string login, string password, string host, int port, int paramSet);
    }
}
