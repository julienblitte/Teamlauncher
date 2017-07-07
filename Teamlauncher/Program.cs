using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

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
            if ((args.Length >= 1) && (args.Contains<string>("-startup")))
            {
                if (getInstance() == 0)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Form MainForm = new Teamlauncher(false);
                    Application.Run();
                }
            }
            else if (getInstance() != 0)
            {
                MessageBox.Show("The application is already running", "Teamlauncher",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Teamlauncher());
            }
        }

        static int getInstance()
        {
            Process [] pList;
            int[] PIDList;
            int PIDCurrent;
            int i;

            pList = Process.GetProcessesByName(Program.name);
            PIDList = new int[pList.Length];
            PIDCurrent = Process.GetCurrentProcess().Id;

            for (i = 0; i < pList.Length; i++)
            {
                PIDList[i] = pList[i].Id;
            }
            Array.Sort(pList, delegate (Process p1, Process p2) { return (DateTime.Compare(p1.StartTime, p2.StartTime)); });

            for(i=0; i < pList.Length; i++)
            {
                if (pList[i].Id == PIDCurrent)
                    return i;
            }
            return -1;
        }
    }
}
