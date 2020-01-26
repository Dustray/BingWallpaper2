using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Core.Utilities
{
    public class BitmapUtil
    {
        public Bitmap Zip(Bitmap bmpSource, int width,int height)
        {
            var orWidth = bmpSource.Width;
            var orHeight = bmpSource.Height;

            var xInterval = (float)orWidth / width;
            var yInterval = (float)orHeight / height;

            Bitmap bmpTarget = new Bitmap(width, height);
            for (int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    var color = bmpSource.GetPixel(Convert.ToInt32(xInterval*x), Convert.ToInt32(yInterval *y));
                    bmpTarget.SetPixel(x, y, color);
                }
            }

            //Graphics g = Graphics.FromImage(bmpTarget);
            //g.DrawImage(bmpSource, 0, 0, bmpSource.Width, bmpSource.Height);
            return bmpTarget;
        }
    }
}
