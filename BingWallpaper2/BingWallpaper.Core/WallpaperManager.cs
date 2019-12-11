using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace BingWallpaper.Core
{
    class WallpaperManager
    {
        public string GetURL()
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

        private void SetWallpaper()
        {
            var bingUrl = GetURL();
            if (string.IsNullOrEmpty(bingUrl))
                return;
            var imageFolderPath = CoreEngine.Current.AppSetting.GetImagePath;
            string imageFilePath = Path.Combine(imageFolderPath, $"bing{DateTime.Now.ToString("yyyyMMdd")}.jpg");
            if (!File.Exists(imageFilePath))//不存在文件
            {
                Bitmap bmpWallpaper;
                WebRequest webreq = WebRequest.Create(bingUrl);
                WebResponse webres = webreq.GetResponse();
                using (Stream stream = webres.GetResponseStream())
                {

                    bmpWallpaper = (Bitmap)Image.FromStream(stream);
                    //stream.Close();
                    if (!Directory.Exists(imageFolderPath))
                    {
                        Directory.CreateDirectory(imageFolderPath);
                    }
                    bmpWallpaper.Save(imageFilePath, ImageFormat.Jpeg);
                    //Console.WriteLine(ConfigOperation.getXmlValue(path, "ImageSavePath"));
                }
            }
            SystemParametersInfo(20, 1, imageFilePath, 1);
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
