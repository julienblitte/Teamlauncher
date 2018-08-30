using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Teamlauncher
{
    class ProgramSingleRun
    {
        public static Process GetRunningInstance()
        {
            Process[] pList;
            int PIDCurrent;
            int i;

            pList = Process.GetProcessesByName(Program.name);
            PIDCurrent = Process.GetCurrentProcess().Id;

            for (i = 0; i < pList.Length; i++)
            {
                if (pList[i].Id != PIDCurrent)
                {
                    return pList[i];
                }
            }

            return null;
        }

        public delegate bool EnumThreadWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool EnumWindows(EnumThreadWindowsProc callback, IntPtr extraData);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowTextLength(IntPtr hWnd);


        private static int targetPid;
        private static String targetText;
        private static IntPtr resultHandle;

        private static bool EnunmFilter(IntPtr hWnd, IntPtr lParam)
        {
            int pid;
            int size;
            StringBuilder sb;

            GetWindowThreadProcessId(hWnd, out pid);
            if (pid == targetPid)
            {
                size = GetWindowTextLength(hWnd);
                if (size > 0)
                {
                    sb = new StringBuilder(size + 1);
                    GetWindowText(hWnd, sb, size + 1);
                    if (sb.ToString().Contains(targetText))
                    {
                        resultHandle = hWnd;
                        return false;
                    }
                }
            }
            return true;
        }

        public static IntPtr FindPidWindows(int pid, String windowText)
        {
            targetPid = pid;
            targetText = windowText;
            resultHandle = IntPtr.Zero;

            EnumWindows(EnunmFilter, IntPtr.Zero);
            return resultHandle;
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
        private const int SW_SHOWNORMAL = 1;
        private const int SW_RESTORE = 9;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        public static void BringToFront(IntPtr window)
        {
            ShowWindow(window, SW_SHOWNORMAL);
            SetActiveWindow(window);
            SetForegroundWindow(window);
        }

        public static void OnNotification(Action<String> mes)
        {
            Thread t;

            t = new Thread(NotificationServer);
            t.Start(mes);
        }
        public static void NotificationServer(object arg)
        {
            PipeServer pipe;
            Action<String> mess;

            mess = (Action<String>)arg;
            pipe = new PipeServer();
            pipe.PipeMessage += mess;
            pipe.Listen("Teamlauncher");
        }
        
        public static bool Notify()
        {
            int PIDCurrent;
            PipeClient pipe;

            PIDCurrent = Process.GetCurrentProcess().Id;

            pipe = new PipeClient();
            try
            {
                pipe.Send(PIDCurrent.ToString(), "Teamlauncher", 1000);
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }
    }
}
