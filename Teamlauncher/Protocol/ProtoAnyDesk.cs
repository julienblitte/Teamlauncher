using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Teamlauncher
{
    class ProtoAnyDesk : RemoteProtocol
    {
        protected string clientExe;
        protected string clientVer;
        protected bool is64;

        public ProtoAnyDesk()
        {
            icon = Properties.Resources.anydesk;
            name = "anydesk";

            /* default values */
            is64 = false;
            clientExe = "";
            clientVer = "";

            /* is64 */
            is64 = Environment.Is64BitOperatingSystem;

            /* Teamviewer */
            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + (is64 ? @"Wow6432Node\" : "") + @"Microsoft\Windows\CurrentVersion\Uninstall\AnyDesk"))
                {
                    clientExe = (string)registryKey.GetValue("InstallLocation");
                    clientVer = (string)registryKey.GetValue("DisplayVersion");
                }
                if (clientExe != "")
                {
                    if (clientExe.StartsWith("\"") && clientExe.EndsWith("\""))
                    {
                        clientExe = clientExe.Substring(1, clientExe.Length-2);
                    }
                    if (!clientExe.EndsWith("\\"))
                    {
                        clientExe += "\\";
                    }
                    clientExe += "AnyDesk.exe";
                }
            }
            catch (Exception)
            {
                clientExe = "";
            }
        }
        public override void run(string login, string password, string host, int port, int paramSet)
        {
            if (clientExe != "")
            {
                //TODO: add support of password
                Process.Start(clientExe,
                    String.Format("\"{0}\"", host));
            }
            else
            {
                MessageBox.Show("Unable to find installed version of AnyDesk!\nIs it installed?");
            }
        }
    }
}
