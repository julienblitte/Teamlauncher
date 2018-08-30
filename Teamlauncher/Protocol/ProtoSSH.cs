using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Teamlauncher.Protocol
{
    class ProtoSSH : ProtocolType
    {
        protected string clientExe;

        public override int AllowedParameters
        {
            get
            {
                return ParamLogin | ParamPassword | ParamHost | ParamPort;
            }
        }

        public ProtoSSH()
        {
            icon = Properties.Resources.ssh;
            name = "ssh";
            defaultPort = 22;

            if (File.Exists(Properties.Settings.Default.Putty))
            {
                clientExe = Properties.Settings.Default.Putty;
            }
            else
            {
                clientExe = "";
            }
        }
        public override void run(string login, string password, string host, int port, int paramSet)
        {
            if (clientExe != "")
            {
                String puttyParameters = "";

                if ((paramSet & ProtocolType.ParamLogin) > 0)
                {
                    puttyParameters += String.Format("\"{0}@{1}\"", login, host);
                }
                else
                {
                    puttyParameters += String.Format("\"{0}\"", host);
                }

                if ((paramSet & ProtocolType.ParamPort) > 0)
                {
                    puttyParameters += String.Format(" -P {0}", port);
                }
                if ((paramSet & ProtocolType.ParamPassword) > 0)
                {
                    puttyParameters += String.Format(" -pw \"{0}\"", password);
                }

                Process.Start(clientExe, puttyParameters);
            }
            else
            {
                MessageBox.Show("Unable to find installed version of Putty!\nPlease update .config file.");
            }
        }
    }
}