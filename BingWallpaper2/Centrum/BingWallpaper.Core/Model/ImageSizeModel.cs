using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Core.Model
{
    /// <summary>
    /// 图片尺寸模板
    /// </summary>
    public class ImageSizeModel
    {
        /// <summary>
        /// 图片尺寸模板
        /// </summary>
        public ImageSizeModel(string name, ImageSizeType type)
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
        public ImageSizeType Type { get; set; }
    }
}
