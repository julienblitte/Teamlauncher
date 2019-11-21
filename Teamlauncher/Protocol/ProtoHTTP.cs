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
    class ProtoHTTP : ProtocolType
    {
        protected bool secure;

        public override int AllowedParameters
        {
            get
            {
                return ParamLogin | ParamPassword | ParamHost | ParamPort | ParamResource;
            }
        }

        public ProtoHTTP(bool secure = true)
        {
            icon = Properties.Resources.http;
            this.secure = secure;
            name = (secure ? "https" : "http");
            defaultPort = (secure ? 443 : 80);

            Trace.WriteLine("Protocol module " + name + " loaded");
        }

        public override void run(int paramSet, string login, string password, string host, int port, string resource)
        {
            string URL = (secure ? "https" : "http") + "://";

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

            Process.Start(URL);
        }
    }
}