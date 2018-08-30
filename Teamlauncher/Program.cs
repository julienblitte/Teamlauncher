using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Threading;

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

            // Teamlauncher already running ?
            runningInstance = ProgramSingleRun.GetRunningInstance();
            if (runningInstance != null)
            {
                IntPtr hwnd;

                // ran twice at startup: exit
                if (startup)
                {
                    Debug.WriteLine("Main(): ran twice at startup, exiting");

                    return;
                }

                // window already created: show it up and exit
                hwnd = ProgramSingleRun.FindPidWindows(runningInstance.Id, "Teamlauncher");
                if (hwnd != IntPtr.Zero)
                {
                    Debug.WriteLine("Main(): ran twice showing existing window");
                    ProgramSingleRun.BringToFront(hwnd);
                    return;
                }

                // Not startup mode, no Window existing, ask other program to quit
                Debug.WriteLine("Main(): ran twice notifying other program");
                if (!ProgramSingleRun.Notify())
                {
                    // if it fails, do not continue, alert and exit
                    MessageBox.Show(String.Format("The application is already running.\n(process {0})", runningInstance.Id), "Teamlauncher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

            Teamlauncher MainForm = new Teamlauncher(!startup);

            ProgramSingleRun.OnNotification(
                (String s) => {
                    Debug.WriteLine("ProgramSingleRun.OnNotification() from thread " + Thread.CurrentThread.ManagedThreadId.ToString());
                    MainForm.exit();
                }
            );

            Application.Run();
        }

    }
}
