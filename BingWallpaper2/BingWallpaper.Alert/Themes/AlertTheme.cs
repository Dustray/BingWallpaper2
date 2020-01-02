using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Popup.Themes
{
    /// <summary>
    /// 默认
    /// </summary>
    public struct AlertTheme
    {

        #region 预设主题
        /// <summary>
        /// 默认
        /// </summary>
        public static AlertTheme Default => new AlertTheme(FromString16("#"), FromString16(""));

        #endregion
        /// <summary>
        /// 弹框主题
        /// </summary>
        /// <param name="primaryColor"></param>
        /// <param name="secondaryColor"></param>
        public AlertTheme(Color primaryColor, Color secondaryColor)
        {
            PrimaryColor = primaryColor;
            SecondaryColor = secondaryColor;
        }
        /// <summary>
        /// 主色调
        /// </summary>
        public Color PrimaryColor { get; set; }

        /// <summary>
        /// 副色调
        /// </summary>
        public Color SecondaryColor { get; set; }



        /// <summary>
        /// 16进制字符串转Color对象
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color FromString16(string color)
        {
            return ColorTranslator.FromHtml(color);
        }
        /// <summary>
        /// 色值转Color对象
        /// </summary>
        /// <param name="A"></param>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Color FromArgb(byte A, byte R, byte G, byte B)
        {
            return Color.FromArgb(A, R, G, B);
        }
    }
}
