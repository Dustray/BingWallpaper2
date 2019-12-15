
using BingWallpaper.Core;
using BingWallpaper.Core.Model;
using BingWallpaper.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

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
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitializeUI()
        {
            Title = $"每日必应壁纸2 version_{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
            
            cbImageSize.ItemsSource = CoreEngine.Current.ImageSizeList;
            cbImageSize.DisplayMemberPath = "Name";
            cbImageSize.SelectedIndex = CoreEngine.Current.ImageSizeList.ToList().FindIndex(s => s.Type == CoreEngine.Current.AppSetting.GetSizeMode);

            cbWallpaperStyle.ItemsSource = CoreEngine.Current.WallpaperStyleList;
            cbWallpaperStyle.DisplayMemberPath = "Name";
            cbWallpaperStyle.SelectedIndex = CoreEngine.Current.WallpaperStyleList.ToList().FindIndex(s => s.Type == CoreEngine.Current.AppSetting.GetStyleMode);

            var path = CoreEngine.Current.AppSetting.GetImagePath();
            tbImageSavePath.Text = path;
            ckbAutoRun.IsChecked = new RegeditUtil().GetAutoRun();
            
            ImgPreview.Source = new WPFSupportFormat().ChangeBitmapToImageSource( CoreEngine.Current.GetWallpaperImage().Result);
               
            _doNotInvokeCheckMethod = false;
           
        }

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
            CoreEngine.Current.SetWallpaper();
        }

        /// <summary>
        /// 图片尺寸选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbImageSize_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var item = (ImageSizeModel)cbImageSize.SelectedItem;
            CoreEngine.Current.AppSetting.SetSizeMode(item.Type);
            CoreEngine.Current.SetWallpaper(true);
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

        #endregion

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

        private void BeginInvoke(Action action)
        {
            
            action?.Invoke();
        }
    }
}
