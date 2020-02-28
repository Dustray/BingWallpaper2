
using BingWallpaper.Core;
using BingWallpaper.Core.Model;
using Vicold.Popup;
using BingWallpaper.Utilities;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BingWallpaper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _doNotInvokeCheckMethod = true;
        private bool _isPackUp = true;
        private bool _isWindowNormal = true;
        private TaskBarUtil taskBarUtil;
        /// <summary>
        /// 主窗体
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            SuperEngine.Current.ReloadBackground = SetAppBackground;
            CoreEngine.Current.Logger.Info("============主界面启动，UI初始化开始============");
            InitializeUI();
            CoreEngine.Current.Logger.Info("============主界面启动，UI初始化结束============");

            CoreEngine.Current.Logger.Info("============检查更新开始============");
            CheckUpdate();
            CoreEngine.Current.Logger.Info("============检查更新结束============");
        }

        #region 初始化

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitializeUI()
        {
            //Title = $"必应每日壁纸2 version_{SuperEngine.Current.Version}（预览版）";

            cbImageSize.ItemsSource = CoreEngine.Current.ImageSizeList;
            cbImageSize.DisplayMemberPath = "Name";
            cbImageSize.SelectedIndex = CoreEngine.Current.ImageSizeList.ToList().FindIndex(s => s.Type == CoreEngine.Current.AppSetting.GetSizeMode);

            cbWallpaperStyle.ItemsSource = CoreEngine.Current.WallpaperStyleList;
            cbWallpaperStyle.DisplayMemberPath = "Name";
            cbWallpaperStyle.SelectedIndex = CoreEngine.Current.WallpaperStyleList.ToList().FindIndex(s => s.Type == CoreEngine.Current.AppSetting.GetStyleMode);

            SetAppBackground();
            //ImgPreview.Source = new WPFSupportFormat().ChangeBitmapToImageSource( CoreEngine.Current.GetWallpaperImage());
            _doNotInvokeCheckMethod = false;
            PackUp(_isPackUp);

            //设置托盘图标
            taskBarUtil = new TaskBarUtil(this);
        }

        /// <summary>
        /// 设置应用背景
        /// </summary>
        /// <param name="force">强制从网络获取</param>
        /// <param name="showSuccess">显示获取成功弹框</param>
        private void SetAppBackground(bool force = false,bool showSuccess = false)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                Bitmap bitmap = null;
                using (var work = new BackgroundWorker())
                {
                    work.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object work_sender, RunWorkerCompletedEventArgs work_e) =>
                    {
                        if (null == bitmap)
                        {
                            CoreEngine.Current.Logger.Info("获取图片资源失败");
                            Alert.Show("获取图片资源失败", AlertTheme.Error);
                            return;
                        }
                        CoreEngine.Current.Logger.Info("获取图片资源成功");
                        tbImageCopyright.ToolTip = tbImageCopyright.Text = CoreEngine.Current.AppSetting.GetCopyright;
                        ImgPreview.Source = new WPFSupportFormat().ChangeBitmapToImageSource(bitmap);
                        if (showSuccess)
                        {
                            Alert.Show("获取图片资源成功", AlertTheme.Success);
                        }
                        bitmap?.Dispose();
                    });
                    work.DoWork += new DoWorkEventHandler((object work_sender, DoWorkEventArgs work_e) =>
                    {
                        bitmap = CoreEngine.Current.GetWallpaperImage(force);
                    });
                    work.RunWorkerAsync();
                }
            }));
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        private void CheckUpdate()
        {
            string updatePath = null;
            string updateVersion = null;
            using (var work = new BackgroundWorker())
            {
                work.DoWork += new DoWorkEventHandler(async (object work_sender, DoWorkEventArgs work_e) =>
                {
                    CoreEngine.Current.Logger.Info($"开始检查更新（异步）");
                    var update = await new UpdateUtil().FindNewUpdate();
                    updatePath = update.path;
                    updateVersion = update.version;
                });
                work.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object work_sender, RunWorkerCompletedEventArgs work_e) =>
                {
                    if (null != updatePath)
                    {
                        BrdUpdate.Visibility = Visibility.Visible;
                        BrdAllRedPoint.Visibility = Visibility.Visible;
                        CoreEngine.Current.Logger.Info($"更新：有新版本（{updateVersion}）");
                        BtnUpdate.ToolTip = $"更新：有新版本（{updateVersion}）";
                    }
                    else
                    {
                        CoreEngine.Current.Logger.Info("更新：已是最新版本");
                        BtnUpdate.ToolTip = "更新：已是最新版本";
                    }
                });
                work.RunWorkerAsync();
            }
        }

        #endregion

        #region 窗体事件

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, System.EventArgs e)
        {
            taskBarUtil.Dispose();
        }


        /// <summary>
        /// 窗体状态变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_StateChanged(object sender, System.EventArgs e)
        {
            var w = sender as Window;
            var state = w.WindowState;
            if (state == WindowState.Normal)
            {
                BtnMaxi.Content = ((char)0xEF2E).ToString();
                BtnMaxi.ToolTip = "最大化";
                _isWindowNormal = true;
            }
            else if (state == WindowState.Maximized)
            {
                BtnMaxi.Content = ((char)0xEF2F).ToString();
                BtnMaxi.ToolTip = "向下还原";
                _isWindowNormal = false;
            }
        }
        #endregion

        #region 成员事件

        #region 下方控件

        /// <summary>
        /// 设置壁纸按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetWallpaper_Click(object sender, RoutedEventArgs e)
        {
            CoreEngine.Current.SetWallpaperAsync();
        }

        /// <summary>
        /// 图片尺寸选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbImageSize_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (_doNotInvokeCheckMethod) return;
            var item = (ImageSizeModel)cbImageSize.SelectedItem;
            CoreEngine.Current.AppSetting.SetSizeMode(item.Type);
            CoreEngine.Current.SetWallpaperAsync(true);
        }

        /// <summary>
        /// 壁纸模式选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWallpaperStyle_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (_doNotInvokeCheckMethod) return;
            var item = (WallpaperStyleModel)cbWallpaperStyle.SelectedItem;
            CoreEngine.Current.AppSetting.SetStyleMode(item.Type);
            new RegeditUtil().SetWallpaperStyle(item.Type);
        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReflush_Click(object sender, RoutedEventArgs e)
        {
            CoreEngine.Current.Logger.Info("============刷新按钮点击，UI初始化开始============");
            SetAppBackground(true, true);
            CoreEngine.Current.Logger.Info("============刷新按钮点击，UI初始化结束============");
        }

        #endregion

        #region 左上方工具栏

        /// <summary>
        /// 设置按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenSetting_Click(object sender, RoutedEventArgs e)
        {
            //创建启动对象
            // System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            // startInfo.UseShellExecute = true;
            // startInfo.WorkingDirectory = Environment.CurrentDirectory;
            // startInfo.FileName = System.Windows.Forms.Application.ExecutablePath;
            // //设置启动动作,确保以管理员身份运行
            //     startInfo.Verb = "runas";
            //System.Diagnostics.Process.Start(startInfo);
            var sw = new SettingWindow();
            sw.Owner = this;
            sw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            sw.ShowInTaskbar = false;
            sw.Show();
        }
        /// <summary>
        /// 升级按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var uw = new UpdateWindow();
            uw.Owner = this;
            uw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            uw.ShowInTaskbar = false;
            uw.Show();
        }

        /// <summary>
        /// 打开图片文件夹按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenImageFolder_Click(object sender, RoutedEventArgs e)
        {
            string v_OpenFolderPath = CoreEngine.Current.AppSetting.GetImagePath();
            System.Diagnostics.Process.Start("explorer.exe", v_OpenFolderPath);
        }

        /// <summary>
        /// 关于按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow();
            about.Owner = this;
            about.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            about.ShowInTaskbar = false;
            about.Show();
        }

        /// <summary>
        /// 下载按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            var dw = new DownloadWindow();
            // dw.Owner = this;
            dw.Show();
        }
/// <summary>
        /// 收起/展开按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPackUp_Click(object sender, RoutedEventArgs e)
        {
            PackUp(_isPackUp);
        }

        #endregion

        #region 右上方按钮

        /// <summary>
        /// 反馈按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFeedback_Click(object sender, RoutedEventArgs e)
        {
            var proc = new Process();
            proc.StartInfo.FileName = "https://forms.office.com/Pages/ResponsePage.aspx?id=DQSIkWdsW0yxEjajBLZtrQAAAAAAAAAAAAO__cwTRKlUOUFMOVJHRlhTMDhZUDRRU05YMzlHOTFDNy4u";
            proc.Start();
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            var justShutdown = CoreEngine.Current.AppSetting.GetCloseIsShutdown;
            if (justShutdown)
            {
                Close();
            }
            else
            {
                this.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 最大化按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMaxi_Click(object sender, RoutedEventArgs e)
        {
            if (_isWindowNormal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        /// <summary>
        /// 最小化按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMini_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        #endregion

        #region 其他

        /// <summary>
        /// 背景图片拖拽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgPreview_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        #endregion

        #endregion

        #region 成员方法

        /// <summary>
        /// 工具栏收起/展开
        /// </summary>
        /// <param name="isPackUp"></param>
        private void PackUp(bool isPackUp)
        {
            _isPackUp = !_isPackUp;
            foreach (var ctl in SpToolBar.Children)
            {
                var ctlEle = ctl as Grid;
                if (null != ctlEle.Tag && ctlEle.Tag.ToString() == "UnPack") continue;
                ctlEle.Visibility = isPackUp ? Visibility.Collapsed : Visibility.Visible;
            }
            BtnPackUp.Content = isPackUp ? ((char)0xF0D6).ToString() : ((char)0xF0D5).ToString();
            BtnPackUp.ToolTip = isPackUp ? "展开" : "收起";
        }

        #endregion

    }
}
