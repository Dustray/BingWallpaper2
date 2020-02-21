using BingWallpaper.Core.Model;
using BingWallpaper.Core.Utilities;
using BingWallpaper.Popup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Core
{
    /// <summary>
    /// 核心引擎
    /// </summary>
    public class CoreEngine
    {
        #region 单例模式
        private static object _lockObj = new object();
        private static CoreEngine _current;
        /// <summary>
        /// 单例模式
        /// </summary>
        public static CoreEngine Current
        {
            get
            {
                lock (_lockObj)
                {
                    if (_current == null)
                    {
                        _current = new CoreEngine();
                    }
                    return _current;
                }
            }
        }

        #endregion

        #region 私有方法
        private CoreEngine()
        {
            initList();
        }

        private void initList()
        {
            ImageSizeList.Add(new ImageSizeModel("1920*1200", ImageSizeType._1200p));
            ImageSizeList.Add(new ImageSizeModel("1920*1080", ImageSizeType._1080p));
            ImageSizeList.Add(new ImageSizeModel("1366*768", ImageSizeType._720p));

            WallpaperStyleList.Add(new WallpaperStyleModel("填充", WallpaperModeType.Full));
            WallpaperStyleList.Add(new WallpaperStyleModel("适应", WallpaperModeType.Adapt));
            WallpaperStyleList.Add(new WallpaperStyleModel("拉伸", WallpaperModeType.Stretch));
            WallpaperStyleList.Add(new WallpaperStyleModel("平铺", WallpaperModeType.Tiling));
            WallpaperStyleList.Add(new WallpaperStyleModel("居中", WallpaperModeType.Center));
            WallpaperStyleList.Add(new WallpaperStyleModel("跨区", WallpaperModeType.Span));
        }
        #endregion

        /// <summary>
        /// 图片尺寸列表
        /// </summary>
        public ObservableCollection<ImageSizeModel> ImageSizeList { get; set; } = new ObservableCollection<ImageSizeModel>();
        /// <summary>
        /// 壁纸类型列表
        /// </summary>
        public ObservableCollection<WallpaperStyleModel> WallpaperStyleList { get; set; } = new ObservableCollection<WallpaperStyleModel>();
        /// <summary>
        /// 应用设置
        /// </summary>
        //public AppSettingOperation AppSetting { get; private set; } = new AppSettingOperation();
        public NewAppSettingOperation AppSetting { get; private set; } = new NewAppSettingOperation();
        /// <summary>
        /// 程序根目录
        /// </summary>
        public string AppRootDirection { get; set; } = System.Windows.Forms.Application.StartupPath;
        /// <summary>
        /// 日志管理器
        /// </summary>
        public readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 异步设置壁纸
        /// </summary>
        /// <param name="forceFromWeb">强制从网络获取</param>
        public void SetWallpaperAsync(bool forceFromWeb = false)
        {
            Current.Logger.Info($"设置桌面壁纸（异步）");

            var locker = new object();
            var isSuccess = false;
            using (var work = new BackgroundWorker())
            {
                work.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object work_sender, RunWorkerCompletedEventArgs work_e) =>
                {
                    var alertConfig = new AlertConfig()
                    {
                        OnlyOneWindowAllowed = true
                    };
                    if (isSuccess)
                    {
                        Alert.Show("", "设置成功", AlertTheme.Success, alertConfig);
                        Current.Logger.Info($"设置桌面壁纸（异步）成功");
                    }
                    else
                    {
                        Alert.Show("", "设置失败", AlertTheme.Error, alertConfig);
                        Current.Logger.Info($"设置桌面壁纸（异步）失败");
                    }
                });
                work.DoWork += new DoWorkEventHandler((object work_sender, DoWorkEventArgs work_e) =>
                {
                    lock (locker)
                    {
                        isSuccess = new WallpaperManager().SetWallpaper(forceFromWeb);
                    }
                });
                work.RunWorkerAsync();
            }
        }
        /// <summary>
        /// 设置壁纸
        /// </summary>
        /// <param name="forceFromWeb">强制从网络获取</param>
        public void SetWallpaper(bool forceFromWeb = false)
        {
            Current.Logger.Info($"设置桌面壁纸");
            new WallpaperManager().SetWallpaper(forceFromWeb);
        }
        /// <summary>
        /// 获取壁纸图片
        /// </summary>
        /// <param name="forceFromWeb">强制从网络获取</param>
        /// <returns></returns>
        public Bitmap GetWallpaperImage(bool forceFromWeb = false)
        {
            Current.Logger.Info($"获取桌面壁纸Bitmap");
            return new WallpaperManager().GetWallpaperImage(forceFromWeb);
        }

        /// <summary>
        /// 下载壁纸图片
        /// </summary>
        /// <param name="date"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool DownloadWallpaperImage(DateTime date, out string result)
        {
            Current.Logger.Info($"下载桌面壁纸图片");
            return new WallpaperManager().DownloadWallpaperImage(date, out result);
        }
    }
}
