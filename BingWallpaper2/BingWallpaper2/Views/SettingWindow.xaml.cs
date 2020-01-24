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


            ckbAutoSet.IsChecked = CoreEngine.Current.AppSetting.GetAutoSet;//new RegeditUtil().GetAutoSet("autoset");
            ckbAutoStart.IsChecked = CoreEngine.Current.AppSetting.GetAutoStart;// new RegeditUtil().GetAutoSet("autostart");
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
            //if (_doNotInvokeCheckMethod) return;
            //var cb = sender as CheckBox;
            //if (null == cb) return;
            //new RegeditUtil().SetAutoSet(cb.Tag.ToString(), (bool)cb.IsChecked);//设置自动设置
        }
        
        private void ckbQuitIsHidden_Checked(object sender, RoutedEventArgs e)
        {
            if (_doNotInvokeCheckMethod) return;
            var cb = sender as CheckBox;
            if (null == cb) return;
            CoreEngine.Current.AppSetting.SetCloseIsShutdown(!(bool)cb.IsChecked);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();    
        }

        private void HeadBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var isAutoStart = (bool)ckbAutoStart.IsChecked ? "ORDER_AutoStartupTrue" : "ORDER_AutoStartupFalse";
            var isAutoSet = (bool)ckbAutoSet.IsChecked ? "ORDER_AutoSetTrue" : "ORDER_AutoSetFalse";

            var result = AppStartUtil.RunAsAdminWithOrder($"{isAutoStart} {isAutoSet} ORDER_Quit");
            
            Close();
        }
    }
}
