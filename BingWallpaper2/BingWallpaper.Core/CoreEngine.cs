using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Core
{
    /// <summary>
    /// 核心引擎
    /// </summary>
    public class CoreEngine
    {
        #region 单例模式
        private object _lockObj = new object();
        private CoreEngine _current;
        /// <summary>
        /// 单例模式
        /// </summary>
        public CoreEngine Current 
        { 
            get
            {
                lock (_lockObj)
                {
                    if (_current == null)
                    {
                        _current = new CoreEngine();
                    }
                    return _current;
                }
            } 
        }

        #endregion


        private CoreEngine()
        {

        }

    }
}
