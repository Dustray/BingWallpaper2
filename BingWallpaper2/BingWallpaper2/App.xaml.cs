using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BingWallpaper
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //命令模式修改设置
            foreach (string arg in e.Args)
            {
                if (arg == "--setautorun")
                {
                    Environment.Exit(1);
                }
            }
        }
    }
}
