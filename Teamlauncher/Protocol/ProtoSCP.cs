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
    class ProtoSCP : ProtocolType
    {
        protected string clientExe;
        protected string clientVer;
        protected bool is64;

        public override int AllowedParameters
        {
            get
            {
                return ParamLogin | ParamPassword | ParamHost | ParamPort;
            }
        }

        public ProtoSCP()
        {
            icon = Properties.Resources.scp;
            name = "scp";
            defaultPort = 22;

            /* is64 */
            is64 = Environment.Is64BitOperatingSystem;

            /* WinSCP */
            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + (is64 ? @"Wow6432Node\" : "") + @"Microsoft\Windows\CurrentVersion\Uninstall\winscp3_is1"))
                {
                    clientExe = (string)registryKey.GetValue("Inno Setup: App Path");
                    clientVer = (string)registryKey.GetValue("DisplayVersion");
                }
                if (clientExe != "")
                {
                    if (clientExe.StartsWith("\"") && clientExe.EndsWith("\""))
                    {
                        clientExe = clientExe.Substring(1, clientExe.Length - 2);
                    }
					clientExe = Path.Combine(clientExe, "WinSCP.exe");
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
                String WinSCPParameter = "";

                switch(paramSet)
                {
                    case ProtocolType.ParamHost :
                        WinSCPParameter = String.Format("\"scp://{0}\"", host);
                        break;
                    case ProtocolType.ParamHost | ProtocolType.ParamLogin :
                        WinSCPParameter = String.Format("\"scp://{0}@{1}\"", Uri.EscapeDataString(login), host);
                        break;
                    case ProtocolType.ParamHost|ProtocolType.ParamLogin | ProtocolType.ParamPassword:
                        WinSCPParameter = String.Format("\"scp://{0}:{1}@{2}\"",
                            Uri.EscapeDataString(login), Uri.EscapeDataString(password), host);
                        break;
                    case ProtocolType.ParamHost | ProtocolType.ParamPort:
                        WinSCPParameter = String.Format("\"scp://{0}:{1}\"", host, port);
                        break;
                    case ProtocolType.ParamHost | ProtocolType.ParamPort | ProtocolType.ParamLogin:
                        WinSCPParameter = String.Format("\"scp://{0}@{1}:{2}\"", Uri.EscapeDataString(login), host, port);
                        break;
                    case ProtocolType.ParamHost | ProtocolType.ParamPort | ProtocolType.ParamLogin | ProtocolType.ParamPassword:
                        WinSCPParameter = String.Format("\"scp://{0}:{1}@{2}:{3}\"",
                            Uri.EscapeDataString(login), Uri.EscapeDataString(password), host, port);
                        break;
                }
                Process.Start(clientExe, WinSCPParameter);
            }
            else
            {
                MessageBox.Show("Unable to find installed version of WinSCP!");
            }
        }
    }
}