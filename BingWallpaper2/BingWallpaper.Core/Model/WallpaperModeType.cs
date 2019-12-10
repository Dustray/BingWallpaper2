using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Core.Model
{
    /// <summary>
    /// 壁纸模式类型
    /// </summary>
    public enum WallpaperModeType
    {
        /// <summary>
        /// 填充
        /// </summary>
        Full,
        /// <summary>
        /// 自适应
        /// </summary>
        Adapt,
        /// <summary>
        /// 拉伸
        /// </summary>
        Stretch,
        /// <summary>
        /// 平铺
        /// </summary>
        Tiling,
        /// <summary>
        /// 居中
        /// </summary>
        Center,
        /// <summary>
        /// 跨区
        /// </summary>
        Span

    }
}
