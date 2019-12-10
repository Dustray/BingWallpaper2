using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Core.Model
{
    public class ImageSizeModel
    {
        public ImageSizeModel(string name, ImageSizeType type)
        {
            Name = name;
            Type = type;
        }
        public string Name { get; set; }
        public ImageSizeType Type { get; set; }
    }
}
