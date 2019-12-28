
using BingWallpaper.Core;
using BingWallpaper.Core.Model;
using BingWallpaper.Utilities;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace BingWallpaper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _doNotInvokeCheckMethod = true;
        
        /// <summary>
        /// 
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
            Title = $"每日必应壁纸2 version_{SuperEngine.Current.Version}";
            
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
                        BtnUpdate.ToolTip=$"更新：有新版本（{updateVersion}）";
                    }
                    else
                    {
                        BtnUpdate.ToolTip="更新：已是最新版本";
                    }
                });
                work.RunWorkerAsync();
            }
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

        #endregion

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            new UpdateWindow().Show();
        }
    }
}
