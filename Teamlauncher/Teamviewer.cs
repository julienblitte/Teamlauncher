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
    class Teamviewer : RemoteProtocol
    {
        protected string teamviewerExe;
        protected string teamviewerVer;
        protected bool is64;

        public Teamviewer()
        {
            icon = Properties.Resources.teamviewer;
            name = "teamviewer";

            /* default values */
            is64 = false;
            teamviewerExe = "";
            teamviewerVer = "";

            /* is64 */
            is64 = Environment.Is64BitOperatingSystem;

            /* Teamviewer */
            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + (is64 ? @"Wow6432Node\" : "") + @"Microsoft\Windows\CurrentVersion\Uninstall\TeamViewer"))
                {
                    teamviewerExe = (string)registryKey.GetValue("InstallLocation");
                    teamviewerVer = (string)registryKey.GetValue("DisplayVersion");
                }
                if (teamviewerExe != "")
                {
                    teamviewerExe.Replace("\\\\", "\\");
                    if (!teamviewerExe.EndsWith("\\"))
                    {
                        teamviewerExe += "\\";
                    }
                    teamviewerExe += "teamviewer.exe";
                }
            }
            catch (Exception)
            {
                teamviewerExe = "";
            }
        }
        public override void run(string login, string password, string host, int port, int paramSet)
        {
            if (teamviewerExe != "")
            {
                if ((paramSet & RemoteProtocol.ParamPassword) > 0)
                {
                    Process.Start(teamviewerExe,
                        String.Format("-i \"{0}\" --Password \"{1}\"", host, password)
                    );
                }
                else
                {
                    Process.Start(teamviewerExe,
                        String.Format("-i \"{0}\"", host)
                    );
                }
            }
            else
            {
                MessageBox.Show("Unable to find installed version of Teamviewer!\nIs it installed?");
            }
        }
    }
}
