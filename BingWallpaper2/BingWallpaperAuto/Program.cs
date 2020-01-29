using BingWallpaper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingWallpaperAuto
{
    class Program
    {
        static void Main(string[] args)
        {
            CoreEngine.Current.Logger.Info($"开机自动设置壁纸：当前日期{DateTime.Now}");

            try
            {
                CoreEngine.Current.SetWallpaper();
            }
            catch (Exception e)
            {
                CoreEngine.Current.Logger.Error(e, $"开机自动设置壁纸失败");
                //MessageBox.Show("失败" + e.ToString());
                return;
            }
            CoreEngine.Current.Logger.Info($"开机自动设置壁纸成功");
        }
    }
}
