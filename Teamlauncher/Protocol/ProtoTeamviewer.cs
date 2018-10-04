using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teamlauncher.Protocol
{
    class ProtoTeamviewer : ProtocolType
    {
        protected string clientExe;
        protected string clientVer;
        protected bool is64;

        public override int AllowedParameters
        {
            get
            {
                return ParamPassword | ParamHost;
            }
        }

        public ProtoTeamviewer()
        {
            icon = Properties.Resources.teamviewer;
            name = "teamviewer";
            defaultPort = 80;

            /* default values */
            is64 = false;
            clientExe = "";
            clientVer = "";

            /* is64 */
            is64 = Environment.Is64BitOperatingSystem;

            Trace.WriteLine("Protocol module " + name + " loaded");

            /* Teamviewer */
            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + (is64 ? @"Wow6432Node\" : "") + @"Microsoft\Windows\CurrentVersion\Uninstall\TeamViewer"))
                {
                    clientExe = (string)registryKey.GetValue("InstallLocation");
                    clientVer = (string)registryKey.GetValue("DisplayVersion");
                }
                if (clientExe != "")
                {
                    if (clientExe.StartsWith("\"") && clientExe.EndsWith("\""))
                    {
                        clientExe = clientExe.Substring(1, clientExe.Length - 2);
                    }
                    clientExe = Path.Combine(clientExe, "teamviewer.exe");
                }
            }
            catch (Exception)
            {
                clientExe = "";
            }
        }
        public override void run(int paramSet, string login, string password, string host, int port, string resource)
        {
            if (clientExe != "")
            {
                if ((paramSet & ProtocolType.ParamPassword) > 0)
                {
                    Process.Start(clientExe,
                        String.Format("-i \"{0}\" --Password \"{1}\"", host, password)
                    );
                }
                else
                {
                    Process.Start(clientExe,
                        String.Format("-i \"{0}\"", host)
                    );
                }
            }
            else
            {
                MessageBox.Show("Unable to find installed version of Teamviewer!\nIs it installed?");
            }
        }
    }
}
