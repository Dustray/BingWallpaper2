using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Popup
{
    /// <summary>
    /// 
    /// </summary>
    public class AlertConfig
    {
        /// <summary>
        /// 动画持续时间，默认100毫秒
        /// </summary>
        public long AnimationDuration { get; set; } = 100;

        /// <summary>
        /// Alert持续显示时间，计时关闭，默认3000毫秒
        /// </summary>
        public long AlertShowDuration { get; set; } = 3000;

        /// <summary>
        /// 每次只允许一个窗体弹出，默认false
        /// </summary>
        public bool OnlyOneWindowAllowed { get; set; } = false;

        /// <summary>
        /// 弹框关闭回调事件
        /// </summary>
        public Action OnAlertCloseCallback { get; set; }
    }
}
