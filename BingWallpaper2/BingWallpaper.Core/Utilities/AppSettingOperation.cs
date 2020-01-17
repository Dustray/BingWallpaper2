using BingWallpaper.Core.Model;
using BingWallpaper.Core.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Core.Utilities
{
    public class AppSettingOperation
    {
        private const int WALLPAPER_STYLE_FULL = 1;
        private const int WALLPAPER_STYLE_ADAPT = 2;
        private const int WALLPAPER_STYLE_STRETCH = 3;
        private const int WALLPAPER_STYLE_TILING = 4;
        private const int WALLPAPER_STYLE_CENTER = 5;
        private const int WALLPAPER_STYLE_SPAN = 6;

        private const int SIZE_1200P = 1;
        private const int SIZE_1080P = 2;
        private const int SIZE_720P = 3;

        /// <summary>
        /// 获取图片尺寸
        /// </summary>
        public  ImageSizeType GetSizeMode => GetSizeType(Settings.Default.WallpaperSize);
        /// <summary>
        /// 设置图片尺寸
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void SetSizeMode(ImageSizeType type)
        {
            Settings.Default.WallpaperSize = SetSizeType(type);
            Settings.Default.Save();
        }
        /// <summary>
        /// 获取壁纸模式
        /// </summary>
        public  WallpaperModeType GetStyleMode => GetModeType(Settings.Default.WallpaperStyle);
        /// <summary>
        /// 设置壁纸模式
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void SetStyleMode(WallpaperModeType type)
        {
            Settings.Default.WallpaperStyle = SetModeType(type);
            Settings.Default.Save();
        }
        /// <summary>
        /// 获取图片保存路径
        /// </summary>
        public string GetImagePath()
        {
            var path = Settings.Default.ImageSavePath;
            if (string.IsNullOrEmpty(path))
            {
                return Path.Combine(System.Windows.Forms.Application.StartupPath, "Image");
            }
            else
            {
                return path;
            }
        }
        /// <summary>
        /// 设置图片保存路径
        /// </summary>
        /// <param name="path"></param>
        public void SetImagePath(string path)
        {
            Settings.Default.ImageSavePath = path;
            Settings.Default.Save();
        }

        /// <summary>
        /// 设置关闭按钮是否直接关闭程序
        /// </summary>
        public void SetCloseIsShutdown(bool flag)
        {
            Settings.Default.CloseJustClose = flag;
            Settings.Default.Save();
        }
        /// <summary>
        /// 获取关闭按钮是否直接关闭程序
        /// </summary>
        public bool GetCloseIsShutdown => Settings.Default.CloseJustClose;

        #region 私有方法

        /// <summary>
        /// 获取壁纸模式类型
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        private WallpaperModeType GetModeType(int setting)
        {
            switch (setting)
            {
                case WALLPAPER_STYLE_FULL:
                    return WallpaperModeType.Full;
                case WALLPAPER_STYLE_ADAPT:
                    return WallpaperModeType.Adapt;
                case WALLPAPER_STYLE_STRETCH:
                    return WallpaperModeType.Stretch;
                case WALLPAPER_STYLE_TILING:
                    return WallpaperModeType.Tiling;
                case WALLPAPER_STYLE_CENTER:
                    return WallpaperModeType.Center;
                case WALLPAPER_STYLE_SPAN:
                    return WallpaperModeType.Span;
                default:
                    return WallpaperModeType.Full;
            }
        }

        /// <summary>
        /// 设置壁纸模式类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private  int SetModeType(WallpaperModeType type)
        {
            switch (type)
            {
                case WallpaperModeType.Full:
                    return WALLPAPER_STYLE_FULL;
                case WallpaperModeType.Adapt:
                    return WALLPAPER_STYLE_ADAPT;
                case WallpaperModeType.Stretch:
                    return WALLPAPER_STYLE_STRETCH;
                case WallpaperModeType.Tiling:
                    return WALLPAPER_STYLE_TILING;
                case WallpaperModeType.Center:
                    return WALLPAPER_STYLE_CENTER;
                case WallpaperModeType.Span:
                    return WALLPAPER_STYLE_SPAN;
                default:
                    return WALLPAPER_STYLE_FULL;
            }
        }

        /// <summary>
        /// 获取壁纸尺寸类型
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        private  ImageSizeType GetSizeType(int setting)
        {
            switch (setting)
            {
                case SIZE_1200P:
                    return ImageSizeType._1200p;
                case SIZE_1080P:
                    return ImageSizeType._1080p;
                case SIZE_720P:
                    return ImageSizeType._720p;
                default:
                    return ImageSizeType._1080p;
            }
        }

        /// <summary>
        /// 设置壁纸尺寸类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private int SetSizeType(ImageSizeType type)
        {
            switch (type)
            {
                case ImageSizeType._1200p:
                    return SIZE_1200P;
                case ImageSizeType._1080p:
                    return SIZE_1080P;
                case ImageSizeType._720p:
                    return SIZE_720P;
                default:
                    return SIZE_1080P;
            }
        }

        #endregion
    }
}
