using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingWallpaper.Utilities
{
    public class TaskBarUtil
    {
        #region 成员属性
        //创建NotifyIcon对象 
        NotifyIcon notifyicon = new NotifyIcon();
        //创建托盘图标对象 
        Icon ico = new Icon("logo.ico");
        //创建托盘菜单对象 
        ContextMenu notifyContextMenu = new ContextMenu();
        #endregion

        public TaskBarUtil()
        {
            notifyicon.Text = "笑话";
            notifyicon.Icon = ico;
            notifyicon.Visible = true;
        }
    }
}
