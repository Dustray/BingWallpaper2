using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Core.Model
{
    public class WallpaperStyleModel
    {
        public WallpaperStyleModel(string name, WallpaperModeType type)
        {
            Name = name;
            Type = type;
        }
        public string Name { get; set; }
        public WallpaperModeType Type { get; set; }
    }
}
