using BingWallpaper.Core.Model;
using System;
using System.Configuration;
using System.IO;

namespace BingWallpaper.Core.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class NewAppSettingOperation
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
        public ImageSizeType GetSizeMode => GetSizeType(ReadSettingInt32("WallpaperSize", 0));

        /// <summary>
        /// 设置图片尺寸
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void SetSizeMode(ImageSizeType type) => UpdateSettings("WallpaperSize", SetSizeType(type));

        /// <summary>
        /// 获取壁纸模式
        /// </summary>
        public WallpaperModeType GetStyleMode => GetModeType(ReadSettingInt32("WallpaperStyle", 0));
        /// <summary>
        /// 设置壁纸模式
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void SetStyleMode(WallpaperModeType type) => UpdateSettings("WallpaperStyle", SetModeType(type));

        /// <summary>
        /// 获取图片保存路径
        /// </summary>
        public string GetImagePath()
        {
            var path = ReadSetting("ImageSavePath");
            if (string.IsNullOrEmpty(path))
            {
                return Path.Combine(CoreEngine.Current.AppRootDirection, "Image");
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
        public void SetImagePath(string path) => UpdateSettings("ImageSavePath", path);

        /// <summary>
        /// 设置关闭按钮是否直接关闭程序
        /// </summary>
        public void SetCloseIsShutdown(bool flag) => UpdateSettings("CloseJustClose", flag);

        /// <summary>
        /// 获取关闭按钮是否直接关闭程序
        /// </summary>
        public bool GetCloseIsShutdown => ReadSettingBoolean("CloseJustClose", true);


        /// <summary>
        /// 设置程序是否开机自动运行
        /// </summary>
        public void SetAutoStart(bool flag) => UpdateSettings("AutoStart", flag);

        /// <summary>
        /// 获取程序是否开机自动运行
        /// </summary>
        public bool GetAutoStart => ReadSettingBoolean("AutoStart", false);

        /// <summary>
        /// 设置程序是否开机自动设置壁纸
        /// </summary>
        public void SetAutoSet(bool flag) => UpdateSettings("AutoSet", flag);
        /// <summary>
        /// 设置程序是否开机自动设置壁纸
        /// </summary>
        public bool GetAutoSet => ReadSettingBoolean("AutoSet", false);

        /// <summary>
        /// 获取程序是否第一次运行
        /// </summary>
        public bool GetAppFirstStart()
        {
            var re = ReadSettingBoolean("AppFirstStart", true);
            if (re)
            {
                UpdateSettings("AppFirstStart", false);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取图片信息
        /// </summary>
        public string GetCopyright => ReadSetting("TodayImageCopyright");

        /// <summary>
        /// 设置图片信息
        /// </summary>
        /// <param name="copyright"></param>
        public void SetCopyright(string copyright) => UpdateSettings("TodayImageCopyright", copyright);


        /// <summary>
        /// 获取自动设置壁纸时间
        /// </summary>
        public (int hour, int minute) GetAutoSetTime()
        {
            var timeStr = ReadSetting("AutoSetTime", "0005");
            try
            {
                var hourStr = timeStr.Substring(0, 2);
                var hour = int.Parse(hourStr);
                var minuteStr = timeStr.Substring(2, 2);
                var minute = int.Parse(minuteStr);
                return (hour, minute);
            }
            catch (FormatException)
            {
                return (0, 5);
            }
            catch (ArgumentOutOfRangeException)
            {
                return (0, 5);
            }
        }
        /// <summary>
        /// 设置自动设置壁纸时间
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        public void SetAutoSetTime(int hour, int minute) => UpdateSettings("AutoSetTime", $"{hour.ToString().PadLeft(2, '0')}{minute.ToString().PadLeft(2, '0')}");

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
        private int SetModeType(WallpaperModeType type)
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
        private ImageSizeType GetSizeType(int setting)
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

        #region 真诚操作
        private void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
        }

        private string ReadSetting(string key, string def = "")
        {
            try
            {
                ExeConfigurationFileMap filemap = new ExeConfigurationFileMap();
                filemap.ExeConfigFilename = Path.Combine(CoreEngine.Current.AppRootDirection, "Config/Setting.config");//配置文件路径
                var config = ConfigurationManager.OpenMappedExeConfiguration(filemap, ConfigurationUserLevel.None);
                var appSettings = config.AppSettings.Settings;
                string result = appSettings[key].Value ?? def;
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                return def;
            }
        }

        private int ReadSettingInt32(string key, int def = 0)
        {
            var valueStr = ReadSetting(key);
            try
            {
                var value = int.Parse(valueStr);
                return value;
            }
            catch
            {
                return def;
            }
        }

        private bool ReadSettingBoolean(string key, bool def = true)
        {
            var valueStr = ReadSetting(key);
            if (valueStr == "true")
            {
                return true;
            }
            else if (valueStr == "false")
            {
                return false;
            }
            else
            {
                return def;
            }
        }
        private void UpdateSettings(string key, string value)
        {
            try
            {
                ExeConfigurationFileMap filemap = new ExeConfigurationFileMap();
                filemap.ExeConfigFilename = Path.Combine(CoreEngine.Current.AppRootDirection, "Config/Setting.config");//配置文件路径
                var config = ConfigurationManager.OpenMappedExeConfiguration(filemap, ConfigurationUserLevel.None);
                var settings = config.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException ex)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        private void UpdateSettings(string key, bool value)
        {
            UpdateSettings(key, value ? "true" : "false");
        }

        private void UpdateSettings(string key, int value)
        {
            UpdateSettings(key, value.ToString());
        }
        #endregion
    }
}
