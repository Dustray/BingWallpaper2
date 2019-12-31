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
        bool createdNew;
        protected override void OnStartup(StartupEventArgs e)
        {
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, "BingWallpaper", out createdNew);
            if (!createdNew)
            {

            }
        }
    }
}
