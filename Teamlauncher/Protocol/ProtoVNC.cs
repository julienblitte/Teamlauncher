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
    class ProtoVNC : ProtocolType
    {
        protected string UltraVNCExe;
        protected string UltraVNCVer;
        protected string TightVNCExe;
        protected bool is64;

        public override int AllowedParameters
        {
            get
            {
                return ParamPassword | ParamHost | ParamPort | ParamResource;
            }
        }

        public ProtoVNC()
        {
            icon = Properties.Resources.vnc;
            name = "vnc";
            defaultPort = 5900;

            /* is64 */
            is64 = Environment.Is64BitOperatingSystem;

            Trace.WriteLine("Protocol module " + name + " loaded");

            /* UltraVNC */
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\" + (is64 ? @"Wow6432Node\" : "") + @"Microsoft\Windows\CurrentVersion\Uninstall\Ultravnc2_is1"))
                {
                    UltraVNCExe = (string)key.GetValue("Inno Setup: App Path");
                    UltraVNCVer = (string)key.GetValue("DisplayVersion");
                }
                if (UltraVNCExe != "")
                {
                    if (UltraVNCExe.StartsWith("\"") && UltraVNCExe.EndsWith("\""))
                    {
                        UltraVNCExe = UltraVNCExe.Substring(1, UltraVNCExe.Length - 2);
                    }
					UltraVNCExe = Path.Combine(UltraVNCExe, "vncviewer.exe");
                }
            }
            catch (Exception)
            {
                UltraVNCExe = "";
            }

            /* TightVNC */
            if (File.Exists(Properties.Settings.Default.TightVNC))
            {
                TightVNCExe = Properties.Settings.Default.TightVNC;
            }
            else
            {
                TightVNCExe = "";
            }
        }

        public override void run(int paramSet, string login, string password, string host, int port, string resource)
        {
            if (UltraVNCExe != "")
            {
                string UltraVNCParameters;

                UltraVNCParameters = "-autoreconnect";

                if ((paramSet & ProtocolType.ParamPort) > 0)
                {
                    UltraVNCParameters += String.Format(" -connect \"{0}:{1}\"", host, port);
                }
                else
                {
                    UltraVNCParameters += String.Format(" -connect \"{0}\"", host);
                }

                if ((paramSet & ProtocolType.ParamLogin) > 0)
                {
                    if ((paramSet & ProtocolType.ParamPassword) > 0)
                    {
                        UltraVNCParameters += String.Format(" -username \"{0}\" -password \"{1}\"", login, password);
                    }
                    else
                    {
                        UltraVNCParameters += String.Format(" -username \"{0}\"", login);
                    }
                }
                if ((paramSet & ProtocolType.ParamResource) > 0)
                {
                    UltraVNCParameters += " " + resource;
                }
                Process.Start(UltraVNCExe, UltraVNCParameters);
            }
            else if (TightVNCExe != "")
            {
                string TightVNCParameters;

                TightVNCParameters = String.Format("-host={0}", host);
                if ((paramSet & ProtocolType.ParamPort) > 0)
                {
                    TightVNCParameters += String.Format(" -port={0}", port);
                }
                if ((paramSet & ProtocolType.ParamLogin) > 0)
                {
                    if ((paramSet & ProtocolType.ParamPassword) > 0)
                    {
                        TightVNCParameters += String.Format(" -username={0} -password={1}", login, password);
                    }
                    else
                    {
                        TightVNCParameters += String.Format(" -username={0}", login);
                    }
                }
                if ((paramSet & ProtocolType.ParamResource) > 0)
                {
                    TightVNCParameters += " " + resource;
                }
                Process.Start(TightVNCExe, TightVNCParameters);
            }
            else
            {
                MessageBox.Show("Unable to find installed version of UltraVNC nor TightVNC!\nIs it installed?");
            }
        }
    }
}