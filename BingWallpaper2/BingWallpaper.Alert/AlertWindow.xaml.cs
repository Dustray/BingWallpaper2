using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BingWallpaper.Popup
{
    /// <summary>
    /// AlertWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AlertWindow : Window, IDisposable
    {
        private double topPosition;

        private Timer timer;
        private int _timerTount = 0;
        private AlertConfig _alertConfig;

        public AlertWindow(string title, string content, AlertTheme alertType, List<UserButton> userButtonList, AlertConfig alertConfig)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            
            tBlockAlertContent.Text = content;

            if (string.IsNullOrEmpty(title))//标题为空
            {
                //标题栏高度为0
                tBlockAlertTitle.Height = 0;
            }
            else
            {
                tBlockAlertTitle.Text = title;
            }
            if (null == userButtonList || 0 == userButtonList.Count)//按钮为空
            {
                //按钮栏高度为0
                ButtonGroup.Height = 0;
                ButtonGroup.Margin =new  Thickness(0);
            }
            else
            {
                foreach( var btn in userButtonList)
                {
                    ButtonGroup.Children.Add(btn.GenerateControl());
                }
            }
            _alertConfig = alertConfig ?? new AlertConfig() ;

            InitTheme(alertType);
        }

        /// <summary>
        /// 初始化UI
        /// </summary>
        /// <param name="alertType"></param>
        private void InitTheme(AlertTheme alertType)
        {
            MainBorder.Background = new SolidColorBrush(AlertTheme.GetMediaColorFromDrawingColor(alertType.PrimaryBackColor));
            tBlockAlertTitle.Background = new SolidColorBrush(AlertTheme.GetMediaColorFromDrawingColor(alertType.SecondaryBackColor));
            brdCose.Background = new SolidColorBrush(AlertTheme.GetMediaColorFromDrawingColor(alertType.SecondaryBackColor));
            tBlockAlertTitle.Foreground = new SolidColorBrush(AlertTheme.GetMediaColorFromDrawingColor(alertType.TitleColor));
            tBlockAlertContent.Foreground = new SolidColorBrush(AlertTheme.GetMediaColorFromDrawingColor(alertType.ContentColor));
            btnClose.Background = new SolidColorBrush(AlertTheme.GetMediaColorFromDrawingColor(alertType.PullBackColor));
            btnClose.Foreground = new SolidColorBrush(AlertTheme.GetMediaColorFromDrawingColor(alertType._pullBackForeColor));
        }

        #region 交互事件

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AlertWindow self = sender as AlertWindow;
            if (self != null)
            {
                self.UpdateLayout();
                topPosition = SystemParameters.WorkArea.Top;//工作区最上边的值
                self.Left = (SystemParameters.WorkArea.Right - ActualWidth) / 2;
                DoubleAnimation animation = new DoubleAnimation();
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(_alertConfig.AnimationDuration));//设置动画的持续时间
                animation.From = topPosition - self.ActualHeight;
                animation.To = topPosition;
                self.BeginAnimation(TopProperty, animation);//设定动画应用于窗体的Left属性

                _timerTount = 0;
                timer = new Timer();
                timer.Enabled = true;
                timer.Interval = 100;//执行间隔时间,单位为毫秒;此时时间间隔为1分钟  
                timer.Elapsed += new ElapsedEventHandler(OnTimerCallback);
                timer.Start();
            }
        }


        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            CloseAlert();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            timer.Enabled = false;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            timer.Enabled = true;
        }

        #endregion

        #region 自动关闭
        private void OnTimerCallback(object source, ElapsedEventArgs e)
        {
            _timerTount += 100;
            if (_timerTount >= _alertConfig.AlertShowDuration)
            {
                Invoke(this, CloseAlert);
            }
        }
        static void Invoke(Window win, Action a)
        {
            win.Dispatcher.Invoke(a);
        }
        public void CloseAlert()
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(_alertConfig.AnimationDuration));
            animation.Completed += (s, a) => { Close(); Dispose(); };//动画执行完毕，关闭当前窗体
            animation.From = topPosition;
            animation.To = topPosition - ActualHeight;
            BeginAnimation(TopProperty, animation);
        }
        #endregion
        public void Dispose()
        {
            if (null != timer)
            {
                timer.Dispose();
                timer = null;
            }
        }

        private void btnClose_MouseEnter(object sender, MouseEventArgs e)
        {
            btnClose.Content = ((char)0xE96D).ToString() ;
        }

        private void btnClose_MouseLeave(object sender, MouseEventArgs e)
        {
            btnClose.Content = ((char)0xEF2D).ToString();
        }
    }
}
