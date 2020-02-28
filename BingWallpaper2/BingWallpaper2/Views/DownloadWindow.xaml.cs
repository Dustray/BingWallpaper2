using BingWallpaper.Core;
using BingWallpaper.Core.Utilities;
using Vicold.Popup;
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
            dpStart.DisplayDateStart = DateTime.Now.AddDays(-6);
            dpStart.DisplayDateEnd = DateTime.Now;
            dpEnd.DisplayDateStart = DateTime.Now.AddDays(-6);
            dpEnd.DisplayDateEnd = DateTime.Now;
            dpStart.SelectedDate = DateTime.Now.AddDays(-6);
            dpEnd.SelectedDate = DateTime.Now;
            var localImageCount = new FileUtil().GetLocalImagesUrl().Count;
            tbPicCount.Text = $"本地：{localImageCount}张";
            tboxDetail.Text = "*由于Bing接口限制，仅支持下载七天内图片。\r\n";
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

        /// <summary>
        /// 下载按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (null == dpStart.SelectedDate || null == dpEnd.SelectedDate)
            {
                Alert.Show("警告", "日期不能为空", AlertTheme.Warning);
                return;
            }
            var start = dpStart.SelectedDate.Value;
            var end = dpEnd.SelectedDate.Value;
            if (start > end)
            {
                Alert.Show("警告", "开始日期不能大于结束日期", AlertTheme.Warning);
                return;
            }
            btnDownload.Content = "正在下载";
            btnDownload.IsEnabled = false;
            var t1 = new Task(() => Download(start, end));
            t1.Start();
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
                image.Source = wf.ChangeBitmapToImageSource(bUtil.Zip(new System.Drawing.Bitmap(list[i]), 320, 180));//new BitmapImage(new Uri(list[i]));
                var lable = new Label();
                lable.Content = i + 1;
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
        public void ShowLog(string result, string file, int progress)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                tboxDetail.AppendText($"{result}：{file}\r\n");
                tboxDetail.ScrollToEnd();
                pgDownload.Value = progress;
                tbProgress.Text = $"{progress}%";
            }));
        }


        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void Download(DateTime start, DateTime end)
        {

            int interval = new TimeSpan(end.Ticks - start.Ticks).Days + 1;
            float p = 1f;
            for (DateTime date = start; date <= end; date = date.AddDays(1), p++)
            {
                var isSuccess = CoreEngine.Current.DownloadWallpaperImage(date, out string result);
                var progress = Convert.ToInt32((p / interval) * 100);
                ShowLog(result, $"{date.ToString("yyyyMMdd")}.jpg", progress);

            }
            this.Dispatcher.Invoke(new Action(() =>
            {
                btnDownload.Content = "开始下载";
                btnDownload.IsEnabled = true;
                Alert.Show("下载完成", AlertTheme.Success);
                var localImageCount = new FileUtil().GetLocalImagesUrl().Count;
                tbPicCount.Text = $"本地：{localImageCount}张";
            }));
        }
        #endregion


    }
}
