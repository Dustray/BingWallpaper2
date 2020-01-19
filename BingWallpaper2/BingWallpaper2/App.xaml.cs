using BingWallpaper.Utilities;
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
            using (var orderUtil = AppStartUtil.CreateOrderCenter())
            {
                //命令模式修改设置
                foreach (string arg in e.Args)
                {
                    //MessageBox.Show(arg);
                    orderUtil.Run(arg);
                        //Environment.Exit(1);
                }
            }
                
        }
    }
}
