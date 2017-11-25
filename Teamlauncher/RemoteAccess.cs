using System;
using System.Collections.Generic;
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
        public RemoteProtocol protocol;

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

        public RemoteAccess(string Url, Dictionary<string, RemoteProtocol> protocolList, out string name)
        {
            MatchCollection matches;
            int i, j;

            matches = Regex.Matches(Url, "^.*:=[a-zA-Z]+://(([^:@]*)(:[^:@]+)?@)?[^:@]*$");

            name = null;
            if (matches.Count != 1)
            {
                throw new FormatException();
            }

            i = Url.IndexOf(":=");
            name = Url.Substring(0, i);
            Url = Url.Substring(i + 2);

            i = Url.IndexOf("://");
            protocol = protocolList[Url.Substring(0, i)];
            Url = Url.Substring(i + 3);

            i = Url.IndexOf("@");
            if (i == -1)
            {
                login = null;
                password = null;
            }
            else
            {
                login = Url.Substring(0, i);
                j = login.IndexOf(":");
                if (j != -1)
                {
                    password = Uri.UnescapeDataString(login.Substring(j+1));
                    login = Uri.UnescapeDataString(login.Substring(0, j));
                }

                Url = Url.Substring(i + 1);
            }

            i = Url.IndexOf(":");
            if (i == -1)
            {
                host = Uri.UnescapeDataString(Url);
                port = protocol.defaultPort;
            }
            else
            {
                host = Uri.UnescapeDataString(Url.Substring(0, i));
                if (!Int32.TryParse(Url.Substring(i + 1), out port))
                    port = protocol.defaultPort;
            }
        }
    }
}
