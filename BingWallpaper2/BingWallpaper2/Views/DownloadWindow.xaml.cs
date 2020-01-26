using BingWallpaper.Core.Utilities;
using BingWallpaper.Popup;
using BingWallpaper.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// DownloadWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadWindow : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public DownloadWindow()
        {
            InitializeComponent();
            Init();
            //lbPic.ItemsSource = ListLbi();
        }

        private void Init()
        {
            dpStart.DisplayDateStart = new DateTime(2016, 3, 5);
            dpStart.DisplayDateEnd = DateTime.Now;
            dpEnd.DisplayDateStart = new DateTime(2016, 3, 5);
            dpEnd.DisplayDateEnd = DateTime.Now;
            dpStart.SelectedDate = new DateTime(2016,3,5);
            dpEnd.SelectedDate = DateTime.Now;
            var localImageCount = new FileUtil().GetLocalImagesUrl().Count;
            tbPicCount.Text = $"本地：{localImageCount}张";
        }



        #region 成员事件

        /// <summary>
        /// 左键拖动穿透
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeadBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        #endregion

        #region 成员方法

        /// <summary>
        /// 右键下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upload_Click(object sender, RoutedEventArgs e)
        {
           // ListBoxItem lb = lbPic.SelectedItem as ListBoxItem;
            //StackPanel sp = (StackPanel)lb.Content;
            //System.Windows.Controls.Label lable = (System.Windows.Controls.Label)sp.Children[1];
            //System.Windows.MessageBox.Show(lable.Content.ToString());
        }

        /// <summary>
        /// ListBox添加item
        /// </summary>
        /// <returns></returns>

        public List<ListBoxItem> ListLbi()
        {
            var listBoxItem = new List<ListBoxItem>();
            var list = new FileUtil().GetLocalImagesUrl();
            var bUtil = new BitmapUtil();
            var wf = new WPFSupportFormat();
            for (int i = 0; i < list.Count; i++)
            {
                var lbi = new ListBoxItem();
                lbi.HorizontalContentAlignment = HorizontalAlignment.Center;
                var sp = new StackPanel();
                var image = new Image();
                image.Stretch = Stretch.UniformToFill;
                image.Source = wf.ChangeBitmapToImageSource(bUtil.Zip(new System.Drawing.Bitmap(list[i]),320,180));//new BitmapImage(new Uri(list[i]));
                var lable = new Label();
                lable.Content =  i+1;
                sp.Children.Add(image);
                sp.Children.Add(lable);
                lbi.Content = sp;
                listBoxItem.Add(lbi);
            }

            return listBoxItem.ToList();
        }

        /// <summary>
        /// 更新日志
        /// </summary>
        /// <param name="file"></param>
        /// <param name="progress"></param>
        public void ShowLog(string file,int progress)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                tboxDetail.AppendText($"正在下载：{file}\r\n");
                tboxDetail.ScrollToEnd();
                pgDownload.Value = progress;
                tbProgress.Text = $"{progress}%";
            }));
        }

        #endregion

        /// <summary>
        /// 下载按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (dpStart.SelectedDate > dpEnd.SelectedDate)
            {
                Alert.Show("警告", "开始日期不要大于结束日期", AlertTheme.Warning);
                return;
            }

        }
    }
}
