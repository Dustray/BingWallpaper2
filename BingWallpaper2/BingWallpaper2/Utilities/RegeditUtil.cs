using BingWallpaper.Core.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BingWallpaper.Utilities
{
    /// <summary>
    /// 注册表工具
    /// </summary>
    class RegeditUtil
    {
        /// <summary>
        /// 设置注册表壁纸样式
        /// </summary>
        public void SetWallpaperStyle(WallpaperModeType type)
        {
            RegistryKey myRegKey = null;
            try
            {
                //设置墙纸显示方式
                myRegKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

                //赋值
                //注意：在把数值型的数据赋到注册表里面的时候，
                //如果不加引号，则该键值会成为“REG_DWORD”型；
                //如果加上引号，则该键值会成为“REG_SZ”型。
                string tileWallpaper = "";
                string wallpaperStyle = "";
                switch (type)
                {
                    case WallpaperModeType.Full:
                        tileWallpaper = "0";
                        wallpaperStyle = "10";
                        break;
                    case WallpaperModeType.Adapt:
                        tileWallpaper = "0";
                        wallpaperStyle = "6";
                        break;
                    case WallpaperModeType.Stretch:
                        tileWallpaper = "0";
                        wallpaperStyle = "2";
                        break;
                    case WallpaperModeType.Tiling:
                        tileWallpaper = "1";
                        wallpaperStyle = "2";
                        break;
                    case WallpaperModeType.Center:
                        tileWallpaper = "0";
                        wallpaperStyle = "0";
                        break;
                    case WallpaperModeType.Span:
                        tileWallpaper = "0";
                        wallpaperStyle = "22";
                        break;
                    default:
                        tileWallpaper = "0";
                        wallpaperStyle = "10";
                        break;
                }
                myRegKey.SetValue("TileWallpaper", tileWallpaper.ToString());
                myRegKey.SetValue("WallpaperStyle", wallpaperStyle.ToString());

                //关闭该项,并将改动保存到磁盘
                myRegKey.Close();
            }
            catch
            {
                myRegKey?.Close();
            }
        }

        /// <summary>
        /// 设置程序开机自动运行，添加进注册表
        /// </summary>
        /// <param name="autoRun"></param>
        public bool SetAutoRun(bool autoRun)
        {
            RegistryKey reg = null;
            try
            {
                var fileName = Path.Combine(Environment.CurrentDirectory, "AutoRunning.exe");
                if (!File.Exists(fileName)) return false;

                string regName = "BingWallpaperAuto";
                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                {
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                }
                if (autoRun)
                {
                    reg.SetValue(regName, fileName);
                }
                else
                {
                    if (reg.GetValue(regName) != null)
                        reg.DeleteValue(regName);
                }
                reg?.Close();
            }
            catch(NullReferenceException e)
            {
                reg?.Close();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 查询注册表，获取程序是否开机自动运行
        /// </summary>
        public bool GetAutoRun()
        {
            RegistryKey reg = null;
            try
            {
                var fileName = Path.Combine(Environment.CurrentDirectory, "AutoRunning.exe");
                if (!File.Exists(fileName)) return false;

                string regName = "BingWallpaperAuto";
                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                {
                    return false;
                }
                if (reg.GetValue(regName) == null)
                {
                    reg?.Close();
                    return false;
                }
                else
                {
                    reg?.Close();
                    return true;
                }
            }
            catch
            {
                reg?.Close();
                return false;
            }
        }
    }
}
