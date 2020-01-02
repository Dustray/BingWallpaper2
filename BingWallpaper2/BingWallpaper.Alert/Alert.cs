using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Popup
{
    public static class Alert
    {
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="content">弹框主体内容</param>
        public static void Show(string content) => Show(null, content);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        public static void Show(string title,string content) => Show(title, content, AlertType.Default);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="content">弹框主体内容</param>
        /// <param name="alertType">弹框类型</param>
        public static void Show(string content, AlertType alertType) => Show(null, content, alertType);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        /// <param name="alertType">弹框类型</param>
        public static void Show(string title, string content, AlertType alertType) => Show(title, content, alertType, new List<UserButton>(),null);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        /// <param name="alertType">弹框类型</param>
        /// <param name="alertConfig">用户自定义配置</param>
        public static void Show(string title, string content, AlertType alertType, AlertConfig alertConfig) => Show(title, content, alertType, new List<UserButton>() , alertConfig);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        /// <param name="alertType">弹框类型</param>
        /// <param name="userButton">交互按钮</param>
        public static void Show(string title, string content, AlertType alertType, UserButton userButton) => Show(title, content, alertType, new List<UserButton>() { userButton }, null);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        /// <param name="alertType">弹框类型</param>
        /// <param name="userButton">交互按钮</param>
        /// <param name="alertConfig">用户自定义配置</param>
        public static void Show(string title, string content, AlertType alertType, UserButton userButton, AlertConfig alertConfig) => Show(title, content, alertType, new List<UserButton>() { userButton}, alertConfig);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        /// <param name="alertType">弹框类型</param>
        /// <param name="userButton1">交互按钮1</param>
        /// <param name="userButton2">交互按钮2</param>
        /// <param name="alertConfig">用户自定义配置</param>
        public static void Show(string title, string content, AlertType alertType, UserButton userButton1, UserButton userButton2, AlertConfig alertConfig) => Show(title, content, alertType, new List<UserButton> (){ userButton1, userButton2 }, alertConfig);
        /// <summary>
        /// 显示弹框
        /// </summary>
        /// <param name="title">弹框标题</param>
        /// <param name="content">弹框主体内容</param>
        /// <param name="alertType">弹框类型</param>
        /// <param name="userButtonList">交互按钮集合</param>
        /// <param name="alertConfig">用户自定义配置</param>
        public static void Show(string title, string content,AlertType alertType,List<UserButton> userButtonList, AlertConfig alertConfig)
        {
            if (string.IsNullOrEmpty(title))//标题为空
            {
                //标题栏高度为0
            }
            else
            {

            }
            if (null == userButtonList || 0 == userButtonList.Count)//按钮为空
            {
                //按钮栏高度为0
            }
            else
            {

            }
            if (null != alertConfig)//用户配置不为空
            {

            }
        }
    }
}
