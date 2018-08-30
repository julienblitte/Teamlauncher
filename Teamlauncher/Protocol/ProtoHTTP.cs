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
                return ParamLogin | ParamPassword | ParamHost | ParamPort;
            }
        }

        public ProtoHTTP(bool secure = true)
        {
            icon = Properties.Resources.http;
            this.secure = secure;
            name = (secure ? "https" : "http");
            defaultPort = (secure ? 443 : 80);
        }

        public override void run(string login, string password, string host, int port, int paramSet)
        {
            String URL = (secure ? "https" : "http") + "://";

            switch (paramSet)
            {
                case ProtocolType.ParamHost:
                    URL += String.Format("{0}", host);
                    break;
                case ProtocolType.ParamHost | ProtocolType.ParamLogin:
                    URL += String.Format("{0}@{1}", Uri.EscapeDataString(login), host);
                    break;
                case ProtocolType.ParamHost | ProtocolType.ParamLogin | ProtocolType.ParamPassword:
                    URL += String.Format("{0}:{1}@{2}",
                        Uri.EscapeDataString(login), Uri.EscapeDataString(password), host);
                    break;
                case ProtocolType.ParamHost | ProtocolType.ParamPort:
                    URL += String.Format("{0}:{1}", host, port);
                    break;
                case ProtocolType.ParamHost | ProtocolType.ParamPort | ProtocolType.ParamLogin:
                    URL += String.Format("{0}@{1}:{2}", Uri.EscapeDataString(login), host, port);
                    break;
                case ProtocolType.ParamHost | ProtocolType.ParamPort | ProtocolType.ParamLogin | ProtocolType.ParamPassword:
                    URL += String.Format("{0}:{1}@{2}:{3}",
                        Uri.EscapeDataString(login), Uri.EscapeDataString(password), host, port);
                    break;
            }
            URL = "\"" + URL + "\"";

            Process.Start(URL);
        }
    }
}