using BingWallpaper.Core;
using FluentScheduler;

namespace BingWallpaper.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class WatcherJobUtil
    {
        /// <summary>
        /// 
        /// </summary>
        public WatcherJobUtil()
        {
            REG = new Registry();
        }

        /// <summary>
        /// 任务调度注册对象
        /// </summary>
        public Registry REG { get; }
        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            RegistryTask();
            //Reset(); //注册重置任务事件
            JobManager.Initialize(REG);
            JobManager.Start();
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            JobManager.StopAndBlock();
            JobManager.RemoveAllJobs();
        }
        /// <summary>
        /// 任务注册
        /// </summary>
        private void RegistryTask()
        {
            REG.Schedule(() => {
                CoreEngine.Current.SetWallpaperAsync();
                SuperEngine.Current.ReloadBackground?.Invoke(false);
            })
            //.ToRunOnceAt(0, 30)
            //.AndEvery(6)
            //.Hours();

            .ToRunEvery(1)
            .Days()
            .At(0, 5);//.At(9, 15);
        }
    }
}
