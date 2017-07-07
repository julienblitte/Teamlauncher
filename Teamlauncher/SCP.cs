using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teamlauncher
{
    class SCP : RemoteProtocol
    {
        protected string WinSCPExe;
        protected string WinSCPVer;
        protected bool is64;

        public SCP()
        {
            icon = Properties.Resources.scp;
            name = "scp";

            /* is64 */
            is64 = Environment.Is64BitOperatingSystem;

            /* WinSCP */
            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + (is64 ? @"Wow6432Node\" : "") + @"Microsoft\Windows\CurrentVersion\Uninstall\winscp3_is1"))
                {
                    WinSCPExe = (string)registryKey.GetValue("Inno Setup: App Path");
                    WinSCPVer = (string)registryKey.GetValue("DisplayVersion");
                }
                if (WinSCPExe != "")
                {
                    WinSCPExe.Replace("\\\\", "\\");
                    if (!WinSCPExe.EndsWith("\\"))
                    {
                        WinSCPExe += "\\";
                    }
                    WinSCPExe += "WinSCP.exe";
                }
            }
            catch (Exception)
            {
                WinSCPExe = "";
            }
        }
        public override void run(string login, string password, string host, int port, int paramSet)
        {
            if (WinSCPExe != "")
            {
                String WinSCPParameter = "";

                switch(paramSet)
                {
                    case RemoteProtocol.ParamHost :
                        WinSCPParameter = String.Format("\"scp://{0}\"", host);
                        break;
                    case RemoteProtocol.ParamHost | RemoteProtocol.ParamLogin :
                        WinSCPParameter = String.Format("\"scp://{0}@{1}\"", Uri.EscapeDataString(login), host);
                        break;
                    case RemoteProtocol.ParamHost|RemoteProtocol.ParamLogin | RemoteProtocol.ParamPassword:
                        WinSCPParameter = String.Format("\"scp://{0}:{1}@{2}\"",
                            Uri.EscapeDataString(login), Uri.EscapeDataString(password), host);
                        break;
                    case RemoteProtocol.ParamHost | RemoteProtocol.ParamPort:
                        WinSCPParameter = String.Format("\"scp://{0}:{1}\"", host, port);
                        break;
                    case RemoteProtocol.ParamHost | RemoteProtocol.ParamPort | RemoteProtocol.ParamLogin:
                        WinSCPParameter = String.Format("\"scp://{0}@{1}:{2}\"", Uri.EscapeDataString(login), host, port);
                        break;
                    case RemoteProtocol.ParamHost | RemoteProtocol.ParamPort | RemoteProtocol.ParamLogin | RemoteProtocol.ParamPassword:
                        WinSCPParameter = String.Format("\"scp://{0}:{1}@{2}:{3}\"",
                            Uri.EscapeDataString(login), Uri.EscapeDataString(password), host, port);
                        break;
                }
                Process.Start(WinSCPExe, WinSCPParameter);
            }
            else
            {
                MessageBox.Show("Unable to find installed version of WinSCP!");
            }
        }
    }
}