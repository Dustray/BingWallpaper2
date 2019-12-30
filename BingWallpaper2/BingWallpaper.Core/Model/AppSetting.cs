using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Core.Model
{
    /// <summary>
    /// 应用设置实体类
    /// </summary>
    public class AppSetting
    {
        /// <summary>
        /// 壁纸尺寸
        /// </summary>
        public string WallpaperSize { get; set; }
        /// <summary>
        /// 壁纸样式
        /// </summary>
        public string WallpaperStyle { get; set; }
        /// <summary>
        /// 图片保存路径
        /// </summary>
        public string ImageSavePath { get; set; }
    }
}
