using SevenZip;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Update
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = LoadConfig();
            if (null == model)
            {
                MessageBox.Show("更新失败，未找到配置文件。");
                return;
            }
            var filePath = Path.GetFullPath(model.SourceFile/*@"Temp\newupdate.ztmp"*/);
            if (!File.Exists(filePath))
            {
                MessageBox.Show("更新失败，未找到更新文件。");
                return;
            }
            var programPath = Environment.CurrentDirectory;
            var unzipPath = programPath;
            if (IntPtr.Size == 4)
            {
                SevenZipExtractor.SetLibraryPath(model.dllx86/*@"x86\7z.dll"*/);
            }
            else
            {
                SevenZipExtractor.SetLibraryPath(model.dllx64/*@"x64\7z.dll"*/);
            }
            for (int k = 0; k < 3; k++)
            {
                Thread.Sleep(1000);
                //解压
                try
                {
                    using (var tmp = new SevenZipExtractor(filePath))
                    {
                        int i = 0;
                        for ( i = 0; i < tmp.ArchiveFileData.Count; i++)
                        {
                            try
                            {
                                tmp.ExtractFiles(unzipPath, tmp.ArchiveFileData[i].Index);
                            }
                            catch
                            {

                            }
                        }
                        try
                        {
                            Process m_Process = new Process();
                            m_Process.StartInfo.FileName = Path.GetFullPath(model.LauncherProgram/*$"BingWallpaper2.exe"*/);
                            m_Process.Start();
                            Environment.Exit(0);
                        }
                        catch
                        {
                            Environment.Exit(0);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("更新失败，缺少更新组件。");
                    return;
                }
            }
        }

        /// <summary>
        /// 加载指定XML文件并序列化为指定泛型对象
        /// <para>异步的</para>
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="xmlFilePath">XML文件路径(绝对路径)</param>
        /// <returns>加载结果</returns>
        public static SevenModel LoadConfig()
        {
            int counter = 0;
            var lines = new string[5];
            string line;
            try
            {
                using (var file = new StreamReader(Path.Combine(@"Config\SevenZipConfig.cfg")))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        lines[counter] = line;
                        counter++;
                    }
                    if (counter != 5)
                    {
                        MessageBox.Show("配置文件解析失败，请手动下载程序。");
                        return null;
                    }
                }
            }
            catch
            {
                MessageBox.Show("更新失败，缺少配置文件，请手动下载程序。");
                return null;
            }
            var m = new SevenModel();
            m.SourceFile = lines[0];
            m.TargetFile = lines[1];
            m.dllx86 = lines[2];
            m.dllx64 = lines[3];
            m.LauncherProgram = lines[4];
            return m;
        }
    }
}
