using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Popup
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
        public static AlertTheme Default => new AlertTheme() {
            PrimaryBackColor = FromString16("#FFFFFF"),
            SecondaryBackColor= FromString16("#F4F4F4"),
            TitleColor= FromString16("#444444"),
            ContentColor = FromString16("#333333"),
            PullBackColor= FromString16("#0078D7"),
        };

        /// <summary>
        /// 提醒
        /// </summary>
        public static AlertTheme Remind => new AlertTheme()
        {
            PrimaryBackColor = FromString16("#5D7599"),
            SecondaryBackColor = FromString16("#ABB6C8"),
            TitleColor = FromString16("#DADADA"),
            ContentColor = FromString16("#DADADA"),
            PullBackColor = FromString16("#DADADA"),
        };

        /// <summary>
        /// 成功
        /// </summary>
        public static AlertTheme Success => new AlertTheme()
        {
            PrimaryBackColor = FromString16("#80BEAF"),
            SecondaryBackColor = FromString16("#83DDD1"),
            TitleColor = FromString16("#FFFFFF"),
            ContentColor = FromString16("#FFFFFF"),
            PullBackColor = FromString16("#F5B994"),
        };

        /// <summary>
        /// 警告
        /// </summary>
        public static AlertTheme Warning => new AlertTheme()
        {
            PrimaryBackColor = FromString16("#F7DB70"),
            SecondaryBackColor = FromString16("#EABEBF"),
            TitleColor = FromString16("#75CCE8"),
            ContentColor = FromString16("#A5DEES"),
            PullBackColor = FromString16("#75CCE8"),
        };

        /// <summary>
        /// 错误
        /// </summary>
        public static AlertTheme Error => new AlertTheme()
        {
            PrimaryBackColor = FromString16("#DA2864"),
            SecondaryBackColor = FromString16("#EC6091"),
            TitleColor = FromString16("#8AE1E2"),
            ContentColor = FromString16("#8AE1E2"),
            PullBackColor = FromString16("#16A5A3"),
        };
        #endregion

        #region 成员属性
        /// <summary>
        /// 主背景色
        /// </summary>
        public Color PrimaryBackColor { get; set; }

        /// <summary>
        /// 副背景色
        /// </summary>
        public Color SecondaryBackColor { get; set; }

        /// <summary>
        /// 标题颜色
        /// </summary>
        public Color TitleColor { get; set; }

        /// <summary>
        /// 内容颜色
        /// </summary>
        public Color ContentColor { get; set; }

        /// <summary>
        /// 收起按钮颜色
        /// </summary>
        public Color PullBackColor { get; set; }

        /// <summary>
        /// 收起按钮文字颜色
        /// </summary>
        internal Color _pullBackForeColor => GetContrastColor(PullBackColor);

        #endregion


        #region 成员方法

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

        public static System.Windows.Media.Color GetMediaColorFromDrawingColor(System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static Color GetContrastColor(Color color)
        {
            var light = color.GetBrightness();
            return light > 0.5 ? Color.Black : Color.White;
        }
        #endregion
    }
}
