using System;
using BingWallpaper.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CoreTest
    {
        [TestMethod]
        public void SetWallpaperWeb()
        {
            CoreEngine.Current.SetWallpaper(true);
        }

        [TestMethod]
        public void SetWallpaperLocal()
        {
            CoreEngine.Current.SetWallpaper(false);
        }

        [TestMethod]
        public void SetWallpaperWebAsync()
        {
            CoreEngine.Current.SetWallpaper(true);
        }

        [TestMethod]
        public void SetWallpaperLocalAsync()
        {
            CoreEngine.Current.SetWallpaper(false);
        }
    }
}
