using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Teamlauncher
{
    class ProtocolTelnet : RemoteProtocol
    {
        protected string clientExe;

        public ProtocolTelnet()
        {
            icon = Properties.Resources.telnet;
            name = "telnet";

            clientExe = Path.Combine(Environment.SystemDirectory, "telnet.exe");
        }

        public override void run(string login, string password, string host, int port, int paramSet)
        {
            try
            {
                Process.Start(clientExe, ((paramSet & RemoteProtocol.ParamPort) == 0  ? String.Format("\"{0}\"", host) : String.Format("\"{0}\" {1}", host, port)));
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to run Telnet! Is it installed?\n"+
                    "To install it from admin shell, type both command lines:\n"+
                    "pkgmgr /iu:\"TelnetClient\"\n"+
                    "copy c:\\windows\\system32\\telnet.exe c:\\windows\\syswow64\\");
            }
        }
    }
}