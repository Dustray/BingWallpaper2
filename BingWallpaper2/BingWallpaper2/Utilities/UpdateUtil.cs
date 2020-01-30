using BingWallpaper.Core;
using COSXML.Model.Bucket;
using COSXML.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper.Utilities
{
    /// <summary>
    /// 程序升级检查工具
    /// </summary>
    public class UpdateUtil
    {
        /// <summary>
        /// 查找新版本更新文件路径
        /// </summary>
        /// <returns></returns>
        public Task<(string path, string version)> FindNewUpdate()
        {
            CoreEngine.Current.Logger.Info($"检查更新：查找新版本");
            var tcsResult = new TaskCompletionSource<(string s, string ss)>();
            if(null== SuperEngine.Current.GlobalConfig)
            {
                CoreEngine.Current.Logger.Info($"检查更新失败：更新配置为空");
                tcsResult.SetResult((null, null));
                return tcsResult.Task;
            }
            string fileHeadPath = $@"https://{SuperEngine.Current.GlobalConfig.BucketName}.cos.{SuperEngine.Current.GlobalConfig.Region}.myqcloud.com/";
            CoreEngine.Current.Logger.Info($"检查更新：版本更新文件路径：{fileHeadPath}");
            try
            {
                string bucket = SuperEngine.Current.GlobalConfig.BucketName; //格式：BucketName-APPID
                GetBucketRequest request = new GetBucketRequest(bucket);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //获取 a/ 下的对象
                request.SetPrefix(SuperEngine.Current.GlobalConfig.UpdatePath);
                if (null == SuperEngine.Current.CosXml)
                {
                    tcsResult.SetResult((null, null));
                    return tcsResult.Task;
                }
                request.SetMaxKeys("10");
                CoreEngine.Current.Logger.Info($"检查更新：请求网络");
                //执行请求
                GetBucketResult result = SuperEngine.Current.CosXml.GetBucket(request);
                //请求成功
                var entityList = result.listBucket.contentsList;
                CoreEngine.Current.Logger.Info($"检查更新：请求成功，返回条目数量：{entityList.Count}");
                var list = entityList.OrderByDescending(a => a.lastModified).ToList();
                foreach (var ett in list)
                {
                    var fullPath = Path.Combine(fileHeadPath, ett.key);
                    var extension = Path.GetExtension(fullPath);
                    if (extension == ".bwp")
                    {
                        var fileName = Path.GetFileNameWithoutExtension(fullPath);
                        try
                        {
                            if (SuperEngine.Current.IsVersionNewer(fileName))
                            {
                                //return fullPath;
                                tcsResult.SetResult((fullPath, fileName));
                                return tcsResult.Task;
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
                tcsResult.SetResult((null, null));
                return tcsResult.Task;
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                //请求失败
                CoreEngine.Current.Logger.Error(clientEx, $"检查更新失败：COS客户端异常");
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                //请求失败
                CoreEngine.Current.Logger.Error(serverEx, $"检查更新失败：COS服务端异常");
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
            catch (Exception e)
            {
                CoreEngine.Current.Logger.Error(e, $"检查更新失败：其他异常");
            }
            tcsResult.SetResult((null, null));
            return tcsResult.Task;
        }
    }
}
