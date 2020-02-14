using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Core.Utilities
{
    /// <summary>
    /// 文件管理工具
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// 获取本地已下载图片
        /// </summary>
        /// <returns></returns>
        public List<string> GetLocalImagesUrl()
        {
            var list = new List<string>();
            var imageFolderPath = CoreEngine.Current.AppSetting.GetImagePath();
            DirectoryInfo root = new DirectoryInfo(imageFolderPath);
             FileInfo[] files = root.GetFiles();
            foreach(var u in files)
            {
                var path = u.FullName;
                if (Path.GetExtension(path) == ".jpg")
                {
                    list.Add(path);
                }
            }
            return list;
        }
    }
}
