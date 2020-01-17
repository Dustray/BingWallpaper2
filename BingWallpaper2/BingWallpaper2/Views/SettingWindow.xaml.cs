using BingWallpaper.Core;
using BingWallpaper.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace BingWallpaper
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        // 进程互斥
        private static bool _alreadyhave = false;

        private bool _doNotInvokeCheckMethod = true;
        /// <summary>
        /// 
        /// </summary>
        public SettingWindow()
        {
            InitializeComponent();


            ckbAutoSet.IsChecked = new RegeditUtil().GetAutoSet("autoset");
            ckbAutoStart.IsChecked = new RegeditUtil().GetAutoSet("autostart");
            ckbQuitIsHidden.IsChecked = !CoreEngine.Current.AppSetting.GetCloseIsShutdown;
            _doNotInvokeCheckMethod = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        /// <summary>
        /// 显示
        /// </summary>
        public new void Show()
        {
            if (_alreadyhave)
            {
                Close();
            }
            else
            {
                _alreadyhave = true;
                base.Show();
            }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            _alreadyhave = false;
        }


        private void ckBoxAutoRun_Checked(object sender, RoutedEventArgs e)
        {
            if (_doNotInvokeCheckMethod) return;
            var cb = sender as CheckBox;
            if (null == cb) return;
            new RegeditUtil().SetAutoSet(cb.Tag.ToString(), (bool)cb.IsChecked);//设置自动设置
        }
        
        private void ckbQuitIsHidden_Checked(object sender, RoutedEventArgs e)
        {
            if (_doNotInvokeCheckMethod) return;
            var cb = sender as CheckBox;
            if (null == cb) return;
            CoreEngine.Current.AppSetting.SetCloseIsShutdown(!(bool)cb.IsChecked);
        }
    }
}
