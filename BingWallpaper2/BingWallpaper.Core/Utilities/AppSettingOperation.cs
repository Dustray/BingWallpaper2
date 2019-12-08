using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Core.Utilities
{
    public class AppSettingOperation
    {
        public string GetSize() => Properties.Settings.Default.WallpaperSize;
        public string GetStyle() => Properties.Settings.Default.WallpaperStyle;
        public string GetImagePath() => Properties.Settings.Default.ImageSavePath;


    }
}
