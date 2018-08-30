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
    class ProtoRDP : ProtocolType
    {
        protected string clientExe;

        public override int AllowedParameters
        {
            get
            {
                return ParamLogin | ParamPassword | ParamHost | ParamPort;
            }
        }

        public ProtoRDP()
        {
            icon = Properties.Resources.rdp;
            name = "rdp";
            defaultPort = 3389;

            clientExe = Path.Combine(Environment.SystemDirectory, "mstsc.exe");
        }

        public override void run(string login, string password, string host, int port, int paramSet)
        {
            string temp = Path.GetTempFileName();

            if ((paramSet & ProtocolType.ParamLogin) > 0)
            {
                File.WriteAllText(temp,
                    String.Format("displayconnectionbar:i:1\nfull address:s:{0}\nautoreconnection enabled:i:1\nprompt for credentials:i:0\nusername:s:{1}\n",
                        host, login
                    ));
            }
            else
            {
                File.WriteAllText(temp,
                    String.Format("displayconnectionbar:i:1\nfull address:s:{0}\nautoreconnection enabled:i:1\nprompt for credentials:i:0\n",
                        host));
            }

            try
            {
                Process.Start(clientExe, "\"" + temp + "\" /admin");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to run Windows Remote Destkop!\nIs it installed?");
            }
        }
    }
}