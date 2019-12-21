using BingWallpaper.Core.Utilities;
using BingWallpaper.Models;
using COSXML;
using COSXML.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper
{
    public class SuperEngine
    {
        #region 单例模式最佳实践

        public static SuperEngine Current = SuperEngineHolder.INSTANCE;
        private class SuperEngineHolder
        {
            internal static readonly SuperEngine INSTANCE = new SuperEngine();
        }
        private SuperEngine() => Initialization();

        #endregion

        public UpdateModel GlobalConfig { get; private set; }

        public CosXmlServer CosXml { get; set; }

        public bool IsVersionNewer(string version)
        {
            var oldVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var oldVersionArray = oldVersion.Split('.');
            var newVersionArray = version.Split('.');
            for(int i = 0; i < oldVersionArray.Length|| i < newVersionArray.Length; i++)
            {
                if(int.Parse(newVersionArray[i])> int.Parse(oldVersionArray[i]))
                {
                    return true;
                }
            }
            return false;
        }

        private async void Initialization()
        {
            var fullPath = Path.GetFullPath(@"Config\UpdateConfig.xml");
            if (!File.Exists(fullPath)) return;
            GlobalConfig =await XMLUtility.LoadXMLAsync<UpdateModel>(fullPath);
            CreateCosXML();
        }

        /// <summary>
        /// 初始化腾讯云配置
        /// </summary>
        private void CreateCosXML()
        {
            //初始化 CosXmlConfig 
            string appid = GlobalConfig.AppID;//设置腾讯云账户的账户标识 APPID
            string region = GlobalConfig.Region; //设置一个默认的存储桶地域
            CosXmlConfig config = new CosXmlConfig.Builder()
              .SetConnectionTimeoutMs(GlobalConfig.ConnectionTimeoutMs)  //设置连接超时时间，单位毫秒，默认45000ms
              .SetReadWriteTimeoutMs(GlobalConfig.ReadWriteTimeoutMs)  //设置读写超时时间，单位毫秒，默认45000ms
              .IsHttps(true)  //设置默认 HTTPS 请求
              .SetAppid(appid)  //设置腾讯云账户的账户标识 APPID
              .SetRegion(region)  //设置一个默认的存储桶地域
              .SetDebugLog(true)  //显示日志
              .Build();  //创建 CosXmlConfig 对象

            //初始化 QCloudCredentialProvider，COS SDK 中提供了3种方式：永久密钥、临时密钥、自定义
            QCloudCredentialProvider cosCredentialProvider = null;

            //方式1， 永久密钥
            string secretId = new SecretEtt().SecretId; //"云 API 密钥 SecretId";
            string secretKey = new SecretEtt().SecretKey; //"云 API 密钥 SecretKey";
            long durationSecond = 600;  //每次请求签名有效时长，单位为秒
            cosCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, durationSecond);

            //初始化 CosXmlServer
            CosXml = new CosXmlServer(config, cosCredentialProvider);
        }
    }
}
