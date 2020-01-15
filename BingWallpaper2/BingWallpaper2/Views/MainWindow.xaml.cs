
using BingWallpaper.Core;
using BingWallpaper.Core.Model;
using BingWallpaper.Popup;
using BingWallpaper.Utilities;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

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
            InitializeUI();
            CheckUpdate();
        }

        #region 初始化

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitializeUI()
        {
            Title = $"每日必应壁纸2 version_{SuperEngine.Current.Version}（预览版）";

            cbImageSize.ItemsSource = CoreEngine.Current.ImageSizeList;
            cbImageSize.DisplayMemberPath = "Name";
            cbImageSize.SelectedIndex = CoreEngine.Current.ImageSizeList.ToList().FindIndex(s => s.Type == CoreEngine.Current.AppSetting.GetSizeMode);

            cbWallpaperStyle.ItemsSource = CoreEngine.Current.WallpaperStyleList;
            cbWallpaperStyle.DisplayMemberPath = "Name";
            cbWallpaperStyle.SelectedIndex = CoreEngine.Current.WallpaperStyleList.ToList().FindIndex(s => s.Type == CoreEngine.Current.AppSetting.GetStyleMode);

            var path = CoreEngine.Current.AppSetting.GetImagePath();
            tbImageSavePath.Text = path;
            ckbAutoRun.IsChecked = new RegeditUtil().GetAutoRun();
            Bitmap bitmap = null;
            using (var work = new BackgroundWorker())
            {
                work.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object work_sender, RunWorkerCompletedEventArgs work_e) =>
                {
                    ImgPreview.Source = new WPFSupportFormat().ChangeBitmapToImageSource(bitmap);
                    bitmap?.Dispose();
                });
                work.DoWork += new DoWorkEventHandler((object work_sender, DoWorkEventArgs work_e) =>
                {
                    bitmap = CoreEngine.Current.GetWallpaperImage();
                });
                work.RunWorkerAsync();
            }
            //ImgPreview.Source = new WPFSupportFormat().ChangeBitmapToImageSource( CoreEngine.Current.GetWallpaperImage());
            _doNotInvokeCheckMethod = false;
            PackUp(_isPackUp);

            //设置托盘图标
            taskBarUtil = new TaskBarUtil(this);
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
                        BtnUpdate.ToolTip = $"更新：有新版本（{updateVersion}）";
                    }
                    else
                    {
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

        #endregion

        #region 成员事件

        /// <summary>
        /// 选择路径按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChoosePath_Click(object sender, RoutedEventArgs e)
        {
            var browserDialog = new FolderBrowserDialog();
            if (browserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string chooseDir = browserDialog.SelectedPath.Trim();
                tbImageSavePath.Text = chooseDir;
                CoreEngine.Current.AppSetting.SetImagePath(chooseDir);
            }
        }
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
            var item = (WallpaperStyleModel)cbWallpaperStyle.SelectedItem;
            CoreEngine.Current.AppSetting.SetStyleMode(item.Type);
            new RegeditUtil().SetWallpaperStyle(item.Type);
        }

        /// <summary>
        /// 自动运行选择框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckbAutoRun_Checked(object sender, RoutedEventArgs e)
        {
            if (_doNotInvokeCheckMethod) return;
            if ((bool)ckbAutoRun.IsChecked)
            {
                new RegeditUtil().SetAutoRun(true);//设置自动运行
            }
            else
            {
                new RegeditUtil().SetAutoRun(false);//设置不自动运行
            }
        }

        /// <summary>
        /// 升级按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            new UpdateWindow().Show();
        }

        /// <summary>
        /// 打开图片文件夹按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenImageFolder_Click(object sender, RoutedEventArgs e)
        {
            string v_OpenFolderPath = tbImageSavePath.Text;
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
            about.Show();
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

        /// <summary>
        /// 背景图片拖拽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgPreview_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
            BtnPackUp.ToolTip = isPackUp ? "展开":"收起";
        }
        #endregion

    }
}
