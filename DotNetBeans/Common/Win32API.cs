using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace KMisaki.OSSProject.DotNetBeans.Common
{
    /// <summary>
    /// Windows system 32bit legacy API.
    /// </summary>
    public class Win32API
    {
        private Win32API() { }

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// アクティブウィンドウのプロセスを取得します。
        /// プロセスの取得に失敗した場合は、NULLを返却します。
        /// </summary>
        /// <returns>プロセスオブジェクト</returns>
        public static Process GetForegroundProcess()
        {
            int processid;
            try
            {
                GetWindowThreadProcessId(GetForegroundWindow(), out processid);
                if (0 != processid)
                {
                    Process p = Process.GetProcessById(processid);
                    return p;
                }
                else
                {
                    System.Diagnostics.Debug.Write(processid + ":NaN. Foreground process not found.");
                }
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return null;
        }

        /// <summary>
        /// アクティブウィンドウを設定します。
        /// </summary>
        /// <param name="hWnd">対象のウィンドウハンドラ</param>
        public static void ActiveWindowWithSetWindowPos(IntPtr hWnd)
        {
            // call windows pos handling method sys32 API
            SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE);
            SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0,
                SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE);
            // call foreground window sys32 API
            SetForegroundWindow(hWnd);
        }

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd,
            int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_SHOWWINDOW = 0x0040;

        private const int HWND_TOPMOST = -1;
        private const int HWND_NOTOPMOST = -2;
    }
}
