using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teamlauncher.Protocol
{
    class ProtoRTSP : ProtocolType
    {
        protected string clientExe;
        protected string clientVer;

        protected bool is64;

        public override int AllowedParameters
        {
            get
            {
                return ParamLogin | ParamPassword | ParamHost | ParamPort | ParamResource;
            }
        }

        public ProtoRTSP()
        {
            icon = Properties.Resources.rtsp;
            name = "rtsp";
            defaultPort = 554;

            /* is64 */
            is64 = Environment.Is64BitOperatingSystem;

            //loaded
            Trace.WriteLine("Protocol module " + name + " loaded");

            /* VLC */
            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + (is64 ? @"Wow6432Node\" : "") + @"VideoLAN\VLC"))
                {
                    clientExe = (string)registryKey.GetValue(null);
                    clientVer = (string)registryKey.GetValue("Version");
                }
                if (clientExe != "")
                {
                    if (clientExe.StartsWith("\"") && clientExe.EndsWith("\""))
                    {
                        clientExe = clientExe.Substring(1, clientExe.Length - 2);
                    }
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
                string URL = "rtsp://";

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
                    if (resource.Length > 1)
                    {
                        if (resource[0] == '/')
                        {
                            URL += String.Format("{0}", resource);
                        }
                        else
                        {
                            URL += String.Format("/{0}", resource);
                        }
                    }
                }

                URL = "\"" + URL + "\"";

                Process.Start(clientExe, URL);
            }
        }
    }
}