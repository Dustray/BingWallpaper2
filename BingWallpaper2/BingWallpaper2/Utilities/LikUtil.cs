using BingWallpaper.Core;
using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Utilities
{
    /// <summary>
    /// 创建桌面快捷方式
    /// </summary>
    public class LikUtil
    {
        /// <summary>
        /// 快速创建
        /// </summary>
        /// <returns></returns>
        public static bool FastCreate()
        {
            var lu = new LikUtil();
            var b1 = lu.CreateDeskTopLik(SuperEngine.Current.AppName, "获取必应每日壁纸作为电脑桌面壁纸，并支持开机自动设置。", Path.Combine(CoreEngine.Current.AppRootDirection, $"{SuperEngine.Current.AppName}.exe"), "logo");
            var b2 = lu.CreateDeskTopLik("一键设置壁纸", "一键设置必应每日壁纸", Path.Combine(CoreEngine.Current.AppRootDirection, "AutoRunning.exe"), "autologo");
            return b1 || b2;
        }
        /// <summary>
        /// 创建桌面快捷方式
        /// </summary>
        /// <param name="name">目标名称</param>
        /// <param name="description">目标描述</param>
        /// <param name="targetFilePath">源文件路径</param>
        /// <param name="logoName">logo名称</param>
        /// <returns></returns>
        public bool CreateDeskTopLik(string name,string description,string targetFilePath,string logoName)
        {
            #region 创建桌面快捷方式
            string deskTop = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string dirPath = CoreEngine.Current.AppRootDirection;
            string exePath = Assembly.GetExecutingAssembly().Location;
            //System.Diagnostics.FileVersionInfo exeInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(exePath);
            if (System.IO.File.Exists($@"{deskTop}\{name}.lnk"))
            {
                //  System.IO.File.Delete(string.Format(@"{0}\快捷键名称.lnk",deskTop));//删除原来的桌面快捷键方式
                return false;
            }
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + $"{name}.lnk");
            shortcut.TargetPath = targetFilePath;         //目标文件
            shortcut.WorkingDirectory = dirPath;    //目标文件夹
            shortcut.WindowStyle = 1;               //目标应用程序的窗口状态分为普通、最大化、最小化【1,3,7】
            shortcut.Description = description;   //描述
            shortcut.IconLocation = $@"{dirPath}\Assets\{logoName}.ico";  //快捷方式图标
            shortcut.Arguments = "";
            shortcut.Hotkey = "ALT+X";              // 快捷键
            shortcut.Save();
            return true;
            #endregion
        }
    }
}
