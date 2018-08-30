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
    class ProtoFTP : ProtocolType
    {
        protected string clientExe;
        protected string clientVer;
        protected bool secure;

        protected bool is64;

        public override int AllowedParameters
        {
            get
            {
                return ParamLogin | ParamPassword | ParamHost | ParamPort;
            }
        }

        public ProtoFTP(bool secure = true)
        {
            icon = Properties.Resources.ftp;
            this.secure = secure;
            name = (secure ? "ftps" : "ftp");
            defaultPort = 21;

            /* is64 */
            is64 = Environment.Is64BitOperatingSystem;

            /* FileZilla */
            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + (is64 ? @"Wow6432Node\" : "") + @"Microsoft\Windows\CurrentVersion\Uninstall\FileZilla Client"))
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
					clientExe = Path.Combine(clientExe, "filezilla.exe");
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
                String FileZillaParameters = (secure ? "sftp" : "ftp") + "://";

                switch (paramSet)
                {
                    case ProtocolType.ParamHost:
                        FileZillaParameters += String.Format("{0}", host);
                        break;
                    case ProtocolType.ParamHost | ProtocolType.ParamLogin:
                        FileZillaParameters += String.Format("{0}@{1}", Uri.EscapeDataString(login), host);
                        break;
                    case ProtocolType.ParamHost | ProtocolType.ParamLogin | ProtocolType.ParamPassword:
                        FileZillaParameters += String.Format("{0}:{1}@{2}",
                            Uri.EscapeDataString(login), Uri.EscapeDataString(password), host);
                        break;
                    case ProtocolType.ParamHost | ProtocolType.ParamPort:
                        FileZillaParameters += String.Format("{0}:{1}", host, port);
                        break;
                    case ProtocolType.ParamHost | ProtocolType.ParamPort | ProtocolType.ParamLogin:
                        FileZillaParameters += String.Format("{0}@{1}:{2}", Uri.EscapeDataString(login), host, port);
                        break;
                    case ProtocolType.ParamHost | ProtocolType.ParamPort | ProtocolType.ParamLogin | ProtocolType.ParamPassword:
                        FileZillaParameters += String.Format("{0}:{1}@{2}:{3}",
                            Uri.EscapeDataString(login), Uri.EscapeDataString(password), host, port);
                        break;
                }
                FileZillaParameters = "\"" + FileZillaParameters + "\"";
                Process.Start(clientExe, FileZillaParameters);
            }
            else
            {
                MessageBox.Show("Unable to find installed version of FileZilla!\n");
            }
        }
    }
}