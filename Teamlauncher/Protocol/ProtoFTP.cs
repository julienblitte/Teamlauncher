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
                return ParamLogin | ParamPassword | ParamHost | ParamPort | ParamResource;
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

            Trace.WriteLine("Protocol module " + name + " loaded");

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
        public override void run(int paramSet, string login, string password, string host, int port, string resource)
        {
            if (clientExe != "")
            {
                string URL = (secure ? "sftp" : "ftp") + "://";

                if ((paramSet & ProtocolType.ParamLogin) != 0)
                {
                    if ((paramSet & ProtocolType.ParamPassword) != 0)
                    {
                        URL += String.Format("{0}:{1}@", Uri.EscapeDataString(login), Uri.EscapeDataString(password));
                    }
                    else
                    {
                        URL += String.Format("{0}@", Uri.EscapeDataString(login));
                    }
                }
                URL += host;

                if ((paramSet & ProtocolType.ParamPort) != 0)
                {
                    URL += String.Format(":{0}", port);
                }
                if ((paramSet & ProtocolType.ParamResource) != 0)
                {
                    URL += String.Format("/{0}", resource);
                }

                URL = "\"" + URL + "\"";

                Process.Start(clientExe, URL);
            }
            else
            {
                MessageBox.Show("Unable to find installed version of FileZilla!\n");
            }
        }
    }
}