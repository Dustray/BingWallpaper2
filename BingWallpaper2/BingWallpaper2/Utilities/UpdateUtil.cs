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
    public class UpdateUtil
    {
        /// <summary>
        /// 查找新版本更新文件路径
        /// </summary>
        /// <returns></returns>
        public Task<(string path,string version)> FindNewUpdate()
        {
            var tcsResult = new TaskCompletionSource<(string s, string ss)>();
            string fileHeadPath = $@"https://{SuperEngine.Current.GlobalConfig.BucketName}.cos.{SuperEngine.Current.GlobalConfig.Region}.myqcloud.com/";
            try
            {
                string bucket = SuperEngine.Current.GlobalConfig.BucketName; //格式：BucketName-APPID
                GetBucketRequest request = new GetBucketRequest(bucket);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //获取 a/ 下的对象
                request.SetPrefix(SuperEngine.Current.GlobalConfig.UpdatePath);
                if(null== SuperEngine.Current.CosXml)
                {
                    tcsResult.SetResult((null, null));
                    return tcsResult.Task;
                }
                //执行请求
                GetBucketResult result = SuperEngine.Current.CosXml.GetBucket(request);
                //请求成功
                var entityList = result.listBucket.contentsList;
                foreach (var ett in entityList)
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
                tcsResult.SetResult((null, null)) ;
                return tcsResult.Task;
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                //请求失败
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                //请求失败
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
            return null;
        }
    }
}
