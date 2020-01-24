using BingWallpaper.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace BingWallpaper.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class AppStartUtil:IDisposable
    {
        private Dictionary<string, Action> _orderAction = new Dictionary<string, Action>();
        private AppStartUtil()
        {
            _orderAction.Add("ORDER_AutoStartupTrue", SetAutoStartupTrue);
            _orderAction.Add("ORDER_AutoStartupFalse", SetAutoStartupFalse);
            _orderAction.Add("ORDER_AutoSetTrue", SetAutoSetTrue);
            _orderAction.Add("ORDER_AutoSetFalse", SetAutoSetFalse);
            _orderAction.Add("ORDER_Quit", ()=> { Environment.Exit(0);});
        }
        /// <summary>
        /// 创建命令实体
        /// </summary>
        /// <returns></returns>
        public static AppStartUtil CreateOrderCenter()
        {
            var sett = new AppStartUtil();
            return sett;
        }
        /// <summary>
        /// 执行命令模式
        /// </summary>
        /// <param name="order"></param>
        public void Run(string order)
        {
            _orderAction[order]?.Invoke();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static int RunAsAdminWithOrder(string order)
        {
            Process process = null;
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.Verb = "runas";
            processInfo.FileName = Application.ExecutablePath;
            processInfo.Arguments = order;
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

        #region 命令

        private void SetAutoStartupTrue()
        {
            new RegeditUtil().SetAutoSet("autostart", true);//设置自动设置
            CoreEngine.Current.AppSetting.SetAutoStart(true);
        }
        private void SetAutoStartupFalse()
        {
            new RegeditUtil().SetAutoSet("autostart", false);//设置自动设置
            CoreEngine.Current.AppSetting.SetAutoStart(false);
        }

        private void SetAutoSetTrue()
        {
            new RegeditUtil().SetAutoSet("autoset", true);//设置自动设置
            CoreEngine.Current.AppSetting.SetAutoSet(true);
        }
        private void SetAutoSetFalse()
        {
            new RegeditUtil().SetAutoSet("autoset", false);//设置自动设置
            CoreEngine.Current.AppSetting.SetAutoSet(false);
        }

        #endregion

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            _orderAction.Clear();
            _orderAction = null;
        }
    }
}
