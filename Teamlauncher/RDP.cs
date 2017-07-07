using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Teamlauncher
{
    class RDP : RemoteProtocol
    {
        protected string mstsc;

        public RDP()
        {
            icon = Properties.Resources.rdp;
            name = "rdp";

            mstsc = Environment.SystemDirectory;
            if (!mstsc.EndsWith("\\"))
            {
                mstsc += "\\";
            }
            mstsc += "mstsc.exe";
        }

        public override void run(string login, string password, string host, int port, int paramSet)
        {
            string temp = Path.GetTempFileName();

            if ((paramSet & RemoteProtocol.ParamLogin) > 0)
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
                Process.Start(mstsc, "\"" + temp + "\" /admin");
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to run Windows Remote Destkop!\nIs it installed?");
            }
        }
    }
}