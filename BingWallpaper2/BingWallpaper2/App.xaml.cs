using BingWallpaper.Popup;
using BingWallpaper.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace BingWallpaper
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 程序启动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            Core.CoreEngine.Current.Logger.Info("程序启动");
            using (var orderUtil = AppStartUtil.CreateOrderCenter())
            {
                //命令模式修改设置
                foreach (string arg in e.Args)
                {
                    Core.CoreEngine.Current.Logger.Info($"程序启动参数：{arg}");
                    orderUtil.Run(arg);
                }
            }
#if !DEBUG
            RunOrExit();
            var isFirstRun = Core.CoreEngine.Current.AppSetting.GetAppFirstStart();
            if (isFirstRun)
            {
                Core.CoreEngine.Current.Logger.Info($"程序安装或升级后第一次启动");
                var re = LikUtil.FastCreate(true);
            }
#endif
        }

        #region 检测程序是否重复执行
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        /// 该函数设置由不同线程产生的窗口的显示状态  
        /// </summary>  
        /// <param name="hWnd">窗口句柄</param>  
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>  
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>  
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        /// <summary>  
        ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。  
        ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。   
        /// </summary>  
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>  
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>  
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int SW_SHOWNOMAL = 1;


        static void RunOrExit()
        {
            Process process = RunningInstance();
            if (process != null)
            {
                //MessageBox.Show("应用程序已经在运行中。。。");
                Core.CoreEngine.Current.Logger.Info($"重复打开，应用程序已经在运行中");
                HandleRunningInstance(process);
                //Alert.Show("", "当前已正在运行程序", AlertTheme.Warning, new AlertConfig()
                //{
                //    OnAlertCloseCallback = () =>
                //    {
                //        System.Environment.Exit(0);
                //    }
                //});
                //System.Threading.Thread.Sleep(3000);  
                MessageBox.Show("程序已在运行中，请勿重复开启", "提醒");
                System.Environment.Exit(0);
            }
        }
        private static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, SW_SHOWNOMAL);   //显示  
            SetForegroundWindow(instance.MainWindowHandle); //当到最前端  

        }
        private static Process RunningInstance()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] Processes = Process.GetProcessesByName(currentProcess.ProcessName);
            foreach (Process process in Processes)
            {
                if (process.Id != currentProcess.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == currentProcess.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
