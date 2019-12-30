using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Models
{
    /// <summary>
    /// 程序升级信息模板
    /// </summary>
    public class UpdateModel
    {
        /// <summary>
        /// 腾讯云账户的账户标识 APPID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 存储桶名称
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// 默认的存储桶地域
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 连接超时时间
        /// </summary>
        public int ConnectionTimeoutMs { get; set; }
        /// <summary>
        /// 读写超时时间
        /// </summary>
        public int ReadWriteTimeoutMs { get; set; }
        /// <summary>
        /// 密钥 SecretId
        /// </summary>
        public string SecretId { get; set; } //"临时密钥 SecretId";
        /// <summary>
        /// 密钥 SecretKey
        /// </summary>
        public string SecretKey { get; set; } //"临时密钥 SecretKey";
        /// <summary>
        /// 每次请求签名有效时长，单位为秒
        /// </summary>
        public int DurationSecond { get; set; } //"临时密钥 token";
        /// <summary>
        /// 升级包更新路径
        /// </summary>
        public string UpdatePath { get; set; }
    }
}
