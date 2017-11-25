using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Teamlauncher
{
    static class Program
    {
        public static String name = "teamlauncher";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Process runningInstance;
            bool startup;

            startup = args.Contains<string>("-startup");

            runningInstance = ProgramSingleRun.GetRunningInstance();
            if (runningInstance == null)
            {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Form MainForm = new Teamlauncher(!startup);
                    Application.Run();
            }
            else if (startup)
            {
                IntPtr hwnd;
                hwnd = ProgramSingleRun.FindPidWindows(runningInstance.Id, "Teamlauncher");

                if (hwnd != IntPtr.Zero)
                {
                    ProgramSingleRun.BringToFront(hwnd);
                }
                else
                {
                    MessageBox.Show(String.Format("The application is already running.\n(process {0})", runningInstance.Id), "Teamlauncher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

    }
}
