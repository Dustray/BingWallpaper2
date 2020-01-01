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
            try
            {
                CoreEngine.Current.SetWallpaper();
            }catch(Exception e)
            {

                MessageBox.Show("失败"+e.ToString());
            }
        }
    }
}
