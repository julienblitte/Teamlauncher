using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Teamlauncher
{
    public class RemoteAccess
    {
        public string host;
        public int port;
        public string login;
        public string password;
        public ProtocolType protocol;

        public override string ToString()
        {
            string result;

            result = protocol.name + "://";
            if ((login != "" && login != null) || (password != "" && password != null))
            {
                if (login != "" && login != null) result += Uri.EscapeDataString(login);
                if (password != "" && password != null) result += ":" + Uri.EscapeDataString(password);
                result += "@";
            }
            result += Uri.EscapeDataString(host);

            if (port != protocol.defaultPort)
            {
                result += ":" + port;
            }

            return result;
        }

        public RemoteAccess()
        {
        }

        public RemoteAccess(string Url, Dictionary<string, ProtocolType> protocolList, out string name)
        {
            Match reg;
            int i, j;

            reg = Regex.Match(Url, "^([^:]+:=)?[a-zA-Z]+://([^:@]*(:[^:@]+)?@)?[^:@]+(:[0-9]+)?$");

            Debug.WriteLine("RemoteAccess: new object from \"" + Url + "\"");

            name = null;
            if (!reg.Success)
            {
                throw new FormatException();
            }

            // check if Teamlauncher header exists
            reg = Regex.Match(Url, "^([^:]+):=");
            if (reg.Success)
            {
                i = reg.Groups[0].ToString().Length;

                // retrieve name
                name = Uri.UnescapeDataString(reg.Groups[1].ToString());
                Url = Url.Substring(i);
                Debug.WriteLine("RemoteAccess: name = \"" + name + "\"");
            }

            // get protocol
            reg = Regex.Match(Url, "^([a-zA-Z]+)://");
            if (!reg.Success)
            {
                Debug.WriteLine("RemoteAccess: protocol error");
                throw new FormatException();
            }

            protocol = protocolList[reg.Groups[1].ToString()];
            Url = Url.Substring(protocol.name.Length + 3);
            Debug.WriteLine("RemoteAccess: protocol = \"" + protocol.name + "\"");

            // check if optional login/password group is here
            i = Url.IndexOf("@");
            if (i == -1)
            {
                login = null;
                password = null;
                Debug.WriteLine("RemoteAccess: login is empty");
                Debug.WriteLine("RemoteAccess: password is empty");
            }
            else
            {
                login = Url.Substring(0, i);
                j = login.IndexOf(":");
                if (j != -1)
                {
                    password = Uri.UnescapeDataString(login.Substring(j + 1));
                    login = Uri.UnescapeDataString(login.Substring(0, j));
                }

                Debug.WriteLine("RemoteAccess: login = \"" + login + "\"");
                if (password != null)
                {
                    Debug.WriteLine("RemoteAccess: password exists");
                }
                Url = Url.Substring(i + 1);
            }

            // check if optional port is here
            i = Url.IndexOf(":");
            if (i == -1)
            {
                host = Uri.UnescapeDataString(Url);
                port = protocol.defaultPort;

                Debug.WriteLine("RemoteAccess: host = \"" + host + "\"");
                Debug.WriteLine("RemoteAccess: port is default");
            }
            else
            {
                host = Uri.UnescapeDataString(Url.Substring(0, i));
                if (!Int32.TryParse(Url.Substring(i + 1), out port))
                {
                    port = protocol.defaultPort;
                }
                Debug.WriteLine("RemoteAccess: port = " + port);
                Debug.WriteLine("RemoteAccess: host = \"" + host + "\"");
            }
        }

    }
}
