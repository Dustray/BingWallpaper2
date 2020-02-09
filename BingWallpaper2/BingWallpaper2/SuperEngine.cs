using BingWallpaper.Core;
using BingWallpaper.Core.Utilities;
using BingWallpaper.Models;
using BingWallpaper.Utilities;
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

        #region 引擎属性

        /// <summary>
        /// 应用全局配置
        /// </summary>
        public UpdateModel GlobalConfig { get; private set; }

        /// <summary>
        /// 腾讯云 对象存储配置对象
        /// </summary>
        public CosXmlServer CosXml { get; set; }

        /// <summary>
        /// 当前程序版本
        /// </summary>
        public string Version => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        /// <summary>
        /// 获取程序名称
        /// </summary>
        public string AppName => System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>
        /// 定时任务
        /// </summary>
        public WatcherJobUtil FWatcher { get; private set; }

        /// <summary>
        /// 加载背景图片
        /// </summary>
        public Action ReloadBackground { get; set; }
        #endregion

        #region 引擎方法

        /// <summary>
        /// 是否存在更新
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public bool IsVersionNewer(string version)
        {
            var oldVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var oldVersionArray = oldVersion.Split('.');
            var newVersionArray = version.Split('.');
            for (int i = 0; i < oldVersionArray.Length || i < newVersionArray.Length; i++)
            {
                if (int.Parse(newVersionArray[i]) > int.Parse(oldVersionArray[i]))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 全局初始化
        /// </summary>
        private async void Initialization()
        {
            CoreEngine.Current.Logger.Info("主程序引擎：全局初始化开始");
            var fullPath = Path.Combine(CoreEngine.Current.AppRootDirection,@"Config\UpdateConfig.xml");
            CoreEngine.Current.Logger.Info($"主程序引擎：加载升级配置文件，路径：{fullPath}");
            if (!File.Exists(fullPath)) return;
            GlobalConfig = await XMLUtility.LoadXMLAsync<UpdateModel>(fullPath);
            CreateCosXML();
            //定时器
            CoreEngine.Current.Logger.Info("主程序引擎：设置定时器");
            FWatcher = new WatcherJobUtil();
            FWatcher.Start();
            CoreEngine.Current.Logger.Info("主程序引擎：全局初始化结束");
        }

        /// <summary>
        /// 初始化腾讯云配置
        /// </summary>
        private void CreateCosXML()
        {
            CoreEngine.Current.Logger.Info($"主程序引擎：加载COS配置文件 CosXmlConfig");
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
            CoreEngine.Current.Logger.Info($"主程序引擎：初始化COS");

            //初始化 CosXmlServer
            CosXml = new CosXmlServer(config, cosCredentialProvider);
        }

        #endregion

        #region 窗体创建方法

        //private AboutWindow _aboutWindow;
        //public void CreateAboutWindow()
        //{
        //    if (null != _aboutWindow) return;
        //    _aboutWindow = new AboutWindow();
        //    _aboutWindow.Owner = this;
        //    _aboutWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        //    _aboutWindow.ShowInTaskbar = false;
        //    _aboutWindow.Show();
        //}

        #endregion
    }
}
