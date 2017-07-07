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
    class HTTP : RemoteProtocol
    {
        protected bool secure;

        public HTTP(bool secure = true)
        {
            icon = Properties.Resources.http;
            this.secure = secure;
            name = (secure ? "https" : "http");
        }

        public override void run(string login, string password, string host, int port, int paramSet)
        {
            String URL = (secure ? "https" : "http") + "://";

            switch (paramSet)
            {
                case RemoteProtocol.ParamHost:
                    URL += String.Format("{0}", host);
                    break;
                case RemoteProtocol.ParamHost | RemoteProtocol.ParamLogin:
                    URL += String.Format("{0}@{1}", Uri.EscapeDataString(login), host);
                    break;
                case RemoteProtocol.ParamHost | RemoteProtocol.ParamLogin | RemoteProtocol.ParamPassword:
                    URL += String.Format("{0}:{1}@{2}",
                        Uri.EscapeDataString(login), Uri.EscapeDataString(password), host);
                    break;
                case RemoteProtocol.ParamHost | RemoteProtocol.ParamPort:
                    URL += String.Format("{0}:{1}", host, port);
                    break;
                case RemoteProtocol.ParamHost | RemoteProtocol.ParamPort | RemoteProtocol.ParamLogin:
                    URL += String.Format("{0}@{1}:{2}", Uri.EscapeDataString(login), host, port);
                    break;
                case RemoteProtocol.ParamHost | RemoteProtocol.ParamPort | RemoteProtocol.ParamLogin | RemoteProtocol.ParamPassword:
                    URL += String.Format("{0}:{1}@{2}:{3}",
                        Uri.EscapeDataString(login), Uri.EscapeDataString(password), host, port);
                    break;
            }
            URL = "\"" + URL + "\"";

            Process.Start(URL);
        }
    }
}