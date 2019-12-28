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
        public AboutWindow()
        {
            InitializeComponent();
            tbVersion.Text = SuperEngine.Current.Version;
        }

        private void tbOpenBlog_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var proc = new Process();
            proc.StartInfo.FileName = "https://dustray.cn/";
            proc.Start();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
