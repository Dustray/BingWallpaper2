using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BingWallpaper.Core
{
    class WallpaperManager
    {
        public string GetBingURL()
        {
            string InfoUrl = "http://cn.bing.com/HPImageArchive.aspx?idx=0&n=1";
            string ImageUrl;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(InfoUrl);
                request.Method = "GET"; request.ContentType = "text/html;charset=UTF-8";
                string XmlString;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream myResponseStream = response.GetResponseStream();
                    using (StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8))
                    {
                        XmlString = myStreamReader.ReadToEnd();
                    }
                }
                // 定义正则表达式用来匹配标签
                Regex regImg = new Regex("<Url>(?<imgUrl>.*?)</Url>", RegexOptions.IgnoreCase);
                // 搜索匹配的字符串
                MatchCollection matches = regImg.Matches(XmlString);
                // 取得匹配项列表
                ImageUrl = "http://www.bing.com" + matches[0].Groups["imgUrl"].Value;

                if (CoreEngine.Current.AppSetting.GetSizeMode == Model.ImageSizeType._1080p)
                {
                    ImageUrl = ImageUrl.Replace("1366x768", "1920x1080");
                }
                else if (CoreEngine.Current.AppSetting.GetSizeMode == Model.ImageSizeType._1200p)
                {
                    ImageUrl = ImageUrl.Replace("1366x768", "1920x1200");
                }
                return ImageUrl;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 从本地或网络设置墙纸
        /// </summary>
        /// <param name="forceFromWeb">强制从网络获取</param>
        public void SetWallpaper(bool forceFromWeb=false)
        {
            Thread.Sleep(5000);
            var imageFolderPath = CoreEngine.Current.AppSetting.GetImagePath();
            var imageFilePath = Path.Combine(imageFolderPath, $"bing{DateTime.Now.ToString("yyyyMMdd")}.jpg");
            if (forceFromWeb||!File.Exists(imageFilePath))//本地不存在文件
            {
                var bingUrl = GetBingURL();
                if (string.IsNullOrEmpty(bingUrl))
                    return;
                var webreq = (HttpWebRequest)WebRequest.Create(bingUrl);
                webreq.Method = "Get";
                using (var webres = webreq.GetResponse())//GetResponse
                {
                    using (var stream = webres.GetResponseStream())
                    {
                        var bmpWallpaper = (Bitmap)Image.FromStream(stream);
                        if (!Directory.Exists(imageFolderPath))
                        {
                            Directory.CreateDirectory(imageFolderPath);
                        }
                        bmpWallpaper.Save(imageFilePath, ImageFormat.Jpeg);
                    }
                }
            }
            SystemParametersInfo(20, 1, imageFilePath, 1);
        }

        /// <summary>
        /// 从本地或网络获取当天最新的图片Bitmap
        /// </summary>
        /// <param name="forceFromWeb">强制从网络获取</param>
        /// <returns></returns>
        public Bitmap GetWallpaperImage(bool forceFromWeb = false)
        {
            var imageFolderPath = CoreEngine.Current.AppSetting.GetImagePath();
            var imageFilePath = Path.Combine(imageFolderPath, $"bing{DateTime.Now.ToString("yyyyMMdd")}.jpg");
            if (forceFromWeb || !File.Exists(imageFilePath))//不存在文件
            {
                var bingUrl = GetBingURL();
                if (string.IsNullOrEmpty(bingUrl))
                    return null;
                var webreq = (HttpWebRequest)WebRequest.Create(bingUrl);
                webreq.Method = "Get";
                using (var webres = webreq.GetResponse())//GetResponse
                {
                    using (var stream = webres.GetResponseStream())
                    {
                        var bmpWallpaper = (Bitmap)Image.FromStream(stream);
                        if (!Directory.Exists(imageFolderPath))
                        {
                            Directory.CreateDirectory(imageFolderPath);
                        }
                        bmpWallpaper.Save(imageFilePath, ImageFormat.Jpeg);
                        return bmpWallpaper;
                    }
                }
            }
            else
            {
                Bitmap bitmap;
                try
                {
                    bitmap = new Bitmap(imageFilePath);
                }
                catch
                {
                    return null;//文件读取失败
                }
                return bitmap;
            }
        }
        #region 系统调用
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(
        int uAction,
        int uParam,
        string lpvParam,
        int fuWinIni
        );
        #endregion
    }
}
