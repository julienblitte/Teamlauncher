using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamlauncher
{
    public class ProtoRemoteAccess
    {
        public string host;
        public int port;
        public string login;
        public string password;
        public RemoteProtocol protocol;

        public override string ToString()
        {
            string result;

            result = protocol.name + "://";
            if ((login != "") || (password != ""))
            {
                if (login != "") result += login;
                if (password != "") result += ":****";
                result += "@";
            }
            result += host;

            if (port != 0)
            {
                result += ":" + port;
            }

            return result;
        }
    }
}
