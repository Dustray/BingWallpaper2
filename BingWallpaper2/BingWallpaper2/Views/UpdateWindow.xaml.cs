using BingWallpaper.Utilities;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;

namespace BingWallpaper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateWindow : Window
    {
        /// <summary>
        /// 升级窗体
        /// </summary>
        public UpdateWindow()
        {
            InitializeComponent();
            CheckUpdate();
            Title = $"检查更新 当前{SuperEngine.Current.Version}";
        }
        /// <summary>
        /// 检查更新
        /// </summary>
        private void CheckUpdate()
        {
            string updatePath = null;
            string updateVersion = null;
            using (var work = new BackgroundWorker())
            {
                work.DoWork += new DoWorkEventHandler(async (object work_sender, DoWorkEventArgs work_e) =>
                {
                    var update = await new UpdateUtil().FindNewUpdate();
                    updatePath = update.path;
                    updateVersion = update.version;
                });
                work.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object work_sender, RunWorkerCompletedEventArgs work_e) =>
                {
                    if (null != updatePath)
                    {
                        ThereHasUpdate(updatePath, updateVersion);
                    }
                    else
                    {
                        ThereNoUpdate();
                    }
                });
                work.RunWorkerAsync();
            }
        }
        /// <summary>
        /// 有更新
        /// </summary>
        /// <param name="path"></param>
        /// <param name="version"></param>
        private void ThereHasUpdate(string path,string version)
        {
            TbTitle.Text = $"正在更新（Version {version}）：";
            TbMainContent.Text = "正在下载"; 
            ExecuteUpdate(path);
        }
        /// <summary>
        /// 无更新
        /// </summary>
        private void ThereNoUpdate()
        {
            TbTitle.Text = $"更新（当前{SuperEngine.Current.Version}）";
            TbMainContent.Text = "已是最新版";
            BtnCancel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 执行更新
        /// </summary>
        /// <param name="url"></param>
        private void ExecuteUpdate(string url)
        {
            bool success = false;
            using (var work = new BackgroundWorker())
            {
                work.DoWork += new DoWorkEventHandler(async (object work_sender, DoWorkEventArgs work_e) =>
                {
                    string path = Path.GetFullPath(@"Temp\");
                    string filePath = Path.Combine(path, "newupdate.ztmp");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    // 设置参数
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    //发送请求并获取相应回应数据
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    //直到request.GetResponse()程序才开始向目标网页发送Post请求
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        //创建本地文件写入流
                        using (Stream stream = new FileStream(filePath, FileMode.Create))
                        {
                            byte[] bArr = new byte[1024];
                            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                            while (size > 0)
                            {
                                stream.Write(bArr, 0, size);
                                size = responseStream.Read(bArr, 0, (int)bArr.Length);
                            }
                            success = true;
                        }
                    }
                });
                work.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object work_sender, RunWorkerCompletedEventArgs work_e) =>
                {
                    TbTitle.Text = success? $"点击下方按钮完成更新":"可惜";
                    TbMainContent.Text = success?"下载成功":"下载失败";
                    BtnOK.Visibility = Visibility.Visible;
                });
                work.RunWorkerAsync();
            }
        }

        /// <summary>
        /// 重启
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process m_Process = new Process();
                m_Process.StartInfo.FileName = Path.GetFullPath($"Update.exe");
                m_Process.Start();
                Environment.Exit(0);
            }
            catch
            {
                MessageBox.Show("缺少更新文件，无法完成更新，请手动下载最新程序。");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 手动下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            var proc = new Process();
            proc.StartInfo.FileName = "https://github.com/Dustray/BingWallpaper2/releases";
            proc.Start();
        }

        private void HeadBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
