using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Core.Model
{
    /// <summary>
    /// 壁纸样式模板
    /// </summary>
    public class WallpaperStyleModel
    {
        /// <summary>
        /// 壁纸样式模板
        /// </summary>
        public WallpaperStyleModel(string name, WallpaperModeType type)
        {
            Name = name;
            Type = type;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public WallpaperModeType Type { get; set; }
    }
}
