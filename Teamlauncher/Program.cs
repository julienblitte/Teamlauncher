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

            runningInstance = ProgramSingleRun.GetRunningInstance();
            if ((args.Length >= 1) && (args.Contains<string>("-startup")))
            {
                if (runningInstance == null)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Form MainForm = new Teamlauncher(false);
                    Application.Run();
                }
            }
            else if (runningInstance != null)
            {
                IntPtr hwnd;
                hwnd = ProgramSingleRun.FindPidWindows(runningInstance.Id, "Teamlauncher");
                if (hwnd != IntPtr.Zero)
                {
                    //ShowWindow(runningInstance.MainWindowHandle, SW_RESTORE);
                    ProgramSingleRun.BringToFront(hwnd);
                }
                else
                {
                    MessageBox.Show(String.Format("The application is already running: pid {0}", runningInstance.Id), "Teamlauncher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Teamlauncher());
            }
        }

      }
}
