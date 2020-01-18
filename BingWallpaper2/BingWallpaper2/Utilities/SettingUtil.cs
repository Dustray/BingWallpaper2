using System.Diagnostics;
using System.Windows.Forms;

namespace BingWallpaper.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class SettingUtil
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Arguments"></param>
        /// <returns></returns>
        public static int RunAsAdmin(string Arguments)
        {
            Process process = null;
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.Verb = "runas";
            processInfo.FileName = Application.ExecutablePath;
            processInfo.Arguments = Arguments;
            try
            {
                process = Process.Start(processInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return -1;
            }
            if (process != null)
            {
                process.WaitForExit();
            }
            int ret = process.ExitCode;
            process.Close();
            return ret;
        }
    }
}
