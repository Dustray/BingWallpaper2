using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BingWallpaper.Popup
{
    public static class Alert
    {
        private static bool _isWindowShowing = false;

        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="content">弹框主体内容</param>
        public static void Show(string content)
            => Show(null, content);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        public static void Show(string title, string content)
            => Show(title, content, AlertTheme.Default);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="content">弹框主体内容</param>
        /// <param name="AlertTheme">弹框类型</param>
        public static void Show(string content, AlertTheme AlertTheme)
            => Show(null, content, AlertTheme);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        /// <param name="AlertTheme">弹框类型</param>
        public static void Show(string title, string content, AlertTheme AlertTheme)
            => Show(title, content, AlertTheme, new List<UserButton>(), null);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        /// <param name="AlertTheme">弹框类型</param>
        /// <param name="alertConfig">用户自定义配置</param>
        public static void Show(string title, string content, AlertTheme AlertTheme, AlertConfig alertConfig)
            => Show(title, content, AlertTheme, new List<UserButton>(), alertConfig);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        /// <param name="AlertTheme">弹框类型</param>
        /// <param name="userButton">交互按钮</param>
        public static void Show(string title, string content, AlertTheme AlertTheme, UserButton userButton)
            => Show(title, content, AlertTheme, new List<UserButton>() { userButton }, null);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        /// <param name="AlertTheme">弹框类型</param>
        /// <param name="userButton">交互按钮</param>
        /// <param name="alertConfig">用户自定义配置</param>
        public static void Show(string title, string content, AlertTheme AlertTheme, UserButton userButton, AlertConfig alertConfig)
            => Show(title, content, AlertTheme, new List<UserButton>() { userButton }, alertConfig);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        /// <param name="AlertTheme">弹框类型</param>
        /// <param name="userButton1">交互按钮1</param>
        /// <param name="userButton2">交互按钮2</param>
        /// <param name="alertConfig">用户自定义配置</param>
        public static void Show(string title, string content, AlertTheme AlertTheme, UserButton userButton1, UserButton userButton2, AlertConfig alertConfig)
            => Show(title, content, AlertTheme, new List<UserButton>() { userButton1, userButton2 }, alertConfig);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        /// <param name="AlertTheme">弹框类型</param>
        /// <param name="userButtonList">交互按钮集合</param>
        /// <param name="alertConfig">用户自定义配置</param>
        public static void Show(string title, string content, AlertTheme AlertTheme, List<UserButton> userButtonList, AlertConfig alertConfig)
        {
            if (null != alertConfig && alertConfig.OnlyOneWindowAllowed && _isWindowShowing)
            {
                return;
            }
            //System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            //{
            _isWindowShowing = true;
            new AlertWindow(title, content, AlertTheme, userButtonList, alertConfig).SetOnWindowCloseCallback(OnWindowClose);
            //}));

        }

        internal static void OnWindowClose()
        {
            _isWindowShowing = false;
        }
    }
}
