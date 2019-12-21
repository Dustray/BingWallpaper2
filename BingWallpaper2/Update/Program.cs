using SevenZip;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Update
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = Path.GetFullPath(@"Temp\newupdate.ztmp");
            var unzipPath = Environment.CurrentDirectory;
            if (IntPtr.Size == 4)
            {
                SevenZipExtractor.SetLibraryPath(@"x86\7z.dll");
            }
            else
            {
                SevenZipExtractor.SetLibraryPath(@"x64\7z.dll");
            }
            for (int k = 0; k < 3; k++)
            {
                Thread.Sleep(1000);
                //解压
                try
                {
                    using (var tmp = new SevenZipExtractor(filePath))
                    {
                        for (var i = 0; i < tmp.ArchiveFileData.Count; i++)
                        {
                            tmp.ExtractFiles(unzipPath, tmp.ArchiveFileData[i].Index);
                        }
                    }
                    Process m_Process = new Process();
                    m_Process.StartInfo.FileName = Path.GetFullPath($"BingWallpaper2.exe");
                    m_Process.Start();
                    Environment.Exit(0);
                }
                catch
                {

                }
            }
        }
    }
}
