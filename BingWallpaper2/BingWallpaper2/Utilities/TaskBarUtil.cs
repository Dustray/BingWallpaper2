using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace BingWallpaper.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskBarUtil : IDisposable
    {
        private MainWindow _mainWindow;
        //创建NotifyIcon对象 
        private NotifyIcon _notifyicon = new NotifyIcon();
        //创建托盘菜单对象 
        private ContextMenuStrip _notifyContextMenu = new ContextMenuStrip();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainWindow"></param>
        public TaskBarUtil(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            var debug = "";
#if DEBUG
            debug = "（Debug）";
#endif
            _notifyicon.Text = $"{SuperEngine.Current.AppName} v{SuperEngine.Current.Version}{debug}";// "必应每日壁纸";
            Icon ico = Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath); 
            _notifyicon.Icon = ico;
            _notifyicon.Visible = true; 
            //_notifyicon.BalloonTipText = "必应每日壁纸1";
            //_notifyicon.ShowBalloonTip(2000);
            _notifyicon.MouseClick += notifyIcon_MouseClick;
            LoadMenu();
        }

        private void LoadMenu()
        {
            var settingBtn = _notifyContextMenu.Items.Add("设置");
            settingBtn.Click += new EventHandler(OpenSettingWindow);

            var closeBtn = _notifyContextMenu.Items.Add("退出");
            closeBtn.Click += new EventHandler(Shutdown);

            _notifyicon.ContextMenuStrip = _notifyContextMenu;
        }

        #region 托盘图标事件

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_mainWindow.WindowState == WindowState.Minimized)
                {
                    _mainWindow.WindowState = WindowState.Normal;
                }
                _mainWindow.Visibility = Visibility.Visible;
                _mainWindow.Activate();
            }
        }
        private void Shutdown(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OpenSettingWindow(object sender, EventArgs e)
        {
            new SettingWindow().Show();
        }
        #endregion

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            _notifyContextMenu?.Dispose();
            _notifyicon?.Dispose();
        }
    }
}
