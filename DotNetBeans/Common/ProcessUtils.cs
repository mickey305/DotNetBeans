using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace KMisaki.OSSProject.DotNetBeans.Common
{
    public class ProcessUtils
    {
        private ProcessUtils() { }

        /// <summary>
        /// プロセスを取得する。
        /// </summary>
        /// <param name="pname">取得対象のプロセス名</param>
        /// <returns>取得されたプロセスオブジェクト</returns>
        public static Process GetSingleProcessByName(string pname)
        {
            if (pname == null) pname = "";
            Process[] processes = Process.GetProcessesByName(pname);
            if (processes == null || processes.Length != 1)
            {
                throw new System.InvalidOperationException("some processes found."
                    + " proces-sname: " + pname
                    + ", process-count:" + processes?.Length);
            }
            return processes[0];
        }
    }
}
