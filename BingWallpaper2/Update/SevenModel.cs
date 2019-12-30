using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Update
{
    public class SevenModel
    {
        /// <summary>
        /// 源文件（压缩包）
        /// </summary>
        public string SourceFile { get; set; }
        /// <summary>
        /// 目标文件
        /// </summary>
        public string TargetFile { get; set; }
        /// <summary>
        /// 7zdllx86文件相对路径
        /// </summary>
        public string dllx86 { get; set; }
        /// <summary>
        /// 7zdllx64文件相对路径
        /// </summary>
        public string dllx64 { get; set; }
        /// <summary>
        /// 启动程序
        /// </summary>
        public string LauncherProgram { get; set; }
    }
}
