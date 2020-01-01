using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace BitLockerUI.Utils
{
    /// <summary>
    /// AlertWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AlertWindow : Window, IDisposable
    {
        public int NotifyTimeSpan { get; set; } = 100;
        private double topPosition;
        private Action _onBtn1ClickCallback;
        private Action _onBtn2ClickCallback;


        private Timer timer;
        private int _timerTount = 0;
        public int _timerSeconds = 3000;

        public AlertWindow(string content)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            
            tBlockAlertContent.Text = content;
        }
        public void SetBtn1Click(string btnText, Action onBtn1ClickCallback)
        {
            btn1.Content = btnText;
            btn1.Visibility = Visibility.Visible;
            _onBtn1ClickCallback = onBtn1ClickCallback;
        }
        public void SetBtn2Click(string btnText, Action onBtn2ClickCallback)
        {
            btn2.Content = btnText;
            btn2.Visibility = Visibility.Visible;
            _onBtn2ClickCallback = onBtn2ClickCallback;
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
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(NotifyTimeSpan));//NotifyTimeSpan是自己定义的一个int型变量，用来设置动画的持续时间
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

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            _onBtn1ClickCallback();
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            _onBtn2ClickCallback();
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
        private void OnTimerCallback(object source, ElapsedEventArgs e)
        {
            _timerTount += 100;
            if (_timerTount == _timerSeconds)
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
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(NotifyTimeSpan));
            animation.Completed += (s, a) => { Close(); Dispose(); };//动画执行完毕，关闭当前窗体
            animation.From = topPosition;
            animation.To = topPosition - ActualHeight;
            BeginAnimation(TopProperty, animation);
        }
        public void Dispose()
        {
            if (null != timer)
            {
                timer.Dispose();
                timer = null;
            }
        }
    }
}
