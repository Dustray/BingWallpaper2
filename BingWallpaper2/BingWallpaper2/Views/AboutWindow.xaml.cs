using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BingWallpaper
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : Window
    {
        /// <summary>
        /// 关于窗体
        /// </summary>
        public AboutWindow()
        {
            InitializeComponent();
            tbVersion.Text = $"{SuperEngine.Current.Version}";
            tbAppName.Text = $"{SuperEngine.Current.AppName}";
        }

        /// <summary>
        /// 打开博客网站按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbOpenBlog_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var proc = new Process();
            proc.StartInfo.FileName = "https://dustray.cn/";
            proc.Start();
        }

        /// <summary>
        /// 打开QQ按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbOpenQQ_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var proc = new Process();
            proc.StartInfo.FileName = "tencent://message/?uin=2045375553&Site=必应每日壁纸&Menu=yes";
            proc.Start();
        }


        /// <summary>
        /// 打开导航网站按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbOpenVicold_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var proc = new Process();
            proc.StartInfo.FileName = "https://vicold.com/";
            proc.Start();
        }

        /// <summary>
        /// 关闭窗体按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void HeadBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void tbOpenGithub_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var proc = new Process();
            proc.StartInfo.FileName = "https://github.com/Dustray";
            proc.Start();
        }
    }
}
