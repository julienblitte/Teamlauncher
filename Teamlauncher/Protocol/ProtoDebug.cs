using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teamlauncher.Protocol
{
    class ProtoDebug : ProtocolType
    {
        public override int AllowedParameters
        {
            get
            {
                return ParamLogin | ParamPassword | ParamHost | ParamPort;
            }
        }

        public ProtoDebug()
        {
            icon = Properties.Resources.debug;
            name = "debug";

            Trace.WriteLine("Protocol module "+name+" loaded");
        }
        public override void run(int paramSet, string login, string password, string host, int port, string resource)
        {
            MessageBox.Show(
            "Protocol:" + name + "\n" +
            "Host: " + ((paramSet & ProtocolType.ParamHost) > 0 ? host : "(n/a)") + "\n" +
            "Login:" + ((paramSet & ProtocolType.ParamLogin) > 0 ? login : "(n/a)") + "\n" +
            "Password:" + ((paramSet & ProtocolType.ParamPassword) > 0 ? "(yes)" : "(n/a)") + "\n" +
            "Port:" + ((paramSet & ProtocolType.ParamPort) > 0 ? port.ToString() : "(default)") + "\n" +
            "Resource:" + ((paramSet & ProtocolType.ParamResource) > 0 ? resource : "(n/a)")
            );


            Trace.WriteLine("Parameters: " + String.Format("{0:X}", paramSet));
            Trace.WriteLine("Protocol:" + name);
            Trace.WriteLine("Host: " + ((paramSet & ProtocolType.ParamHost) > 0 ? host : "(n/a)"));
            Trace.WriteLine("Login:" + ((paramSet & ProtocolType.ParamLogin) > 0 ? login : "(n/a)"));
            Trace.WriteLine("Password (b64):" + ((paramSet & ProtocolType.ParamPassword) > 0 ? Convert.ToBase64String(Encoding.UTF8.GetBytes(password)) : "(n/a)"));
            Trace.WriteLine("Port:" + ((paramSet & ProtocolType.ParamPort) > 0 ? port.ToString() : "(default)"));
            Trace.WriteLine("Resource:" + ((paramSet & ProtocolType.ParamResource) > 0 ? resource : "(n/a)"));

            //MessageBox.Show(password);
        }
    }
}
