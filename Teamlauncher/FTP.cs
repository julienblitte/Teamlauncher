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
    class FTP : RemoteProtocol
    {
        protected string FileZillaExe;
        protected string FileZillaVer;
        protected bool secure;

        protected bool is64;

        public FTP(bool secure = true)
        {
            icon = Properties.Resources.ftp;
            this.secure = secure;
            name = (secure ? "sftp" : "ftp");

            /* is64 */
            is64 = Environment.Is64BitOperatingSystem;

            /* FileZilla */
            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + (is64 ? @"Wow6432Node\" : "") + @"Microsoft\Windows\CurrentVersion\Uninstall\FileZilla Client"))
                {
                    FileZillaExe = (string)registryKey.GetValue("InstallLocation");
                    FileZillaVer = (string)registryKey.GetValue("DisplayVersion");
                }
                if (FileZillaExe != "")
                {
                    FileZillaExe.Replace("\\\\", "\\");
                    if (!FileZillaExe.EndsWith("\\"))
                    {
                        FileZillaExe += "\\";
                    }
                    FileZillaExe += "filezilla.exe";
                }
            }
            catch (Exception)
            {
                FileZillaExe = "";
            }
        }
        public override void run(string login, string password, string host, int port, int paramSet)
        {
            if (FileZillaExe != "")
            {
                String FileZillaParameters = (secure ? "sftp" : "ftp") + "://";

                switch (paramSet)
                {
                    case RemoteProtocol.ParamHost:
                        FileZillaParameters += String.Format("{0}", host);
                        break;
                    case RemoteProtocol.ParamHost | RemoteProtocol.ParamLogin:
                        FileZillaParameters += String.Format("{0}@{1}", Uri.EscapeDataString(login), host);
                        break;
                    case RemoteProtocol.ParamHost | RemoteProtocol.ParamLogin | RemoteProtocol.ParamPassword:
                        FileZillaParameters += String.Format("{0}:{1}@{2}",
                            Uri.EscapeDataString(login), Uri.EscapeDataString(password), host);
                        break;
                    case RemoteProtocol.ParamHost | RemoteProtocol.ParamPort:
                        FileZillaParameters += String.Format("{0}:{1}", host, port);
                        break;
                    case RemoteProtocol.ParamHost | RemoteProtocol.ParamPort | RemoteProtocol.ParamLogin:
                        FileZillaParameters += String.Format("{0}@{1}:{2}", Uri.EscapeDataString(login), host, port);
                        break;
                    case RemoteProtocol.ParamHost | RemoteProtocol.ParamPort | RemoteProtocol.ParamLogin | RemoteProtocol.ParamPassword:
                        FileZillaParameters += String.Format("{0}:{1}@{2}:{3}",
                            Uri.EscapeDataString(login), Uri.EscapeDataString(password), host, port);
                        break;
                }
                FileZillaParameters = "\"" + FileZillaParameters + "\"";
                Process.Start(FileZillaExe, FileZillaParameters);
            }
            else
            {
                MessageBox.Show("Unable to find installed version of FileZilla!\n");
            }
        }
    }
}