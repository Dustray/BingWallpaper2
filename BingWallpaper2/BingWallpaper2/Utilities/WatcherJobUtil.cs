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
            //Reset(); //注册重置任务事件
            JobManager.Initialize(REG);
            JobManager.Start();
            RegistryTask();
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            JobManager.Stop();
            JobManager.RemoveAllJobs();
        }
        /// <summary>
        /// 任务注册
        /// </summary>
        private void RegistryTask()
        {
            var time=CoreEngine.Current.AppSetting.GetAutoSetTime();
            JobManager.AddJob(() => {
                SuperEngine.Current.ReloadBackground?.Invoke(false,false);
            }, (s) => s
            //.ToRunOnceAt(0, 30)
            //.AndEvery(6)
            //.Hours();
            .ToRunEvery(1)
            .Days()
            .At(time.hour, time.minute));//.At(9, 15);

            JobManager.AddJob(() => {
                CoreEngine.Current.SetWallpaperAsync();
            }, (s) => s
            //.ToRunOnceAt(0, 30)
            //.AndEvery(6)
            //.Hours();
            .ToRunEvery(1)
            .Days()
            .At(time.hour, time.minute+2));//.At(9, 15);
        }

        public void ResetTask()
        {
            Stop();
            Start();
        }
    }
}
