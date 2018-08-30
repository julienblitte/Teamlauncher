using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Teamlauncher.Protocol
{
    class ProtoAnyDesk : ProtocolType
    {
        protected string clientExe;
        protected string clientVer;
        protected bool is64;

        public override int AllowedParameters
        {
            get
            {
                return ParamPassword | ParamHost | ParamPort;
            }
        }

        public ProtoAnyDesk()
        {
            icon = Properties.Resources.anydesk;
            name = "anydesk";
            defaultPort = 7070;

            /* default values */
            is64 = false;
            clientExe = "";
            clientVer = "";

            /* is64 */
            is64 = Environment.Is64BitOperatingSystem;

            /* AnyDesk */
            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + (is64 ? @"Wow6432Node\" : "") + @"Microsoft\Windows\CurrentVersion\Uninstall\AnyDesk"))
                {
                    clientExe = (string)registryKey.GetValue("InstallLocation");
                    clientVer = (string)registryKey.GetValue("DisplayVersion");
                }
                if (clientExe != "")
                {
                    if (clientExe.StartsWith("\"") && clientExe.EndsWith("\""))
                    {
                        clientExe = clientExe.Substring(1, clientExe.Length-2);
                    }
                    if (!clientExe.EndsWith("\\"))
                    {
                        clientExe += "\\";
                    }
                    clientExe += "AnyDesk.exe";
                }
            }
            catch (Exception)
            {
                clientExe = "";
            }
        }
        public override void run(string login, string password, string host, int port, int paramSet)
        {
			if (clientExe != "")
			{
				ProcessStartInfo startInfo;
				
				startInfo = new ProcessStartInfo();
				startInfo.UseShellExecute = false;
				startInfo.RedirectStandardOutput = false;
				startInfo.RedirectStandardInput = true;
				startInfo.RedirectStandardError = false;
				startInfo.FileName = clientExe;

				if ((paramSet & ProtocolType.ParamPassword) > 0)
				{
					startInfo.Arguments = host + " --with-password";

					Process process = new Process();
					process.StartInfo = startInfo;
					process.Start();

					process.StandardInput.WriteLine(password+"\n");
				}
				else
				{
					startInfo.Arguments = host;

					Process process = new Process();
					process.StartInfo = startInfo;
					process.Start();
				}
				/*

				if ((paramSet & RemoteProtocol.ParamPassword) > 0)
				{
					var temp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bat");

					File.WriteAllText(temp,
						String.Format("@echo {0}|\"{1}\" {2} --with-password\n", password, clientExe, host));

					Process.Start(temp,
						String.Format("\"{0}\"", host));
				}
				else
				{
					Process.Start(clientExe,
						String.Format("\"{0}\"", host));
				}
				*/
			}
			else
			{
				MessageBox.Show("Unable to find installed version of AnyDesk!\nIs it installed?");
			}
        }
    }
}
