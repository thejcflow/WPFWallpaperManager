using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperManager.Models;

namespace WallpaperManager.Utils
{
    public class ImageFileHelper
    {
        private static string folder = Path.GetTempPath();

        public static byte[] ReadFile(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static void WriteFile(byte[] bytes, Wallpaper wallpaper)
        {
            var fileName = string.Format("WM-{0}-{1}", wallpaper.Id, wallpaper.Name);
            var path = Path.Combine(folder, fileName);
            if (!File.Exists(path))
            {
                File.WriteAllBytes(path, bytes);
            }
        }

        public static string GetFilePath(Wallpaper wallpaper)
        {
            var fileName = string.Format("WM-{0}-{1}", wallpaper.Id, wallpaper.Name);
            return Path.Combine(folder, fileName);
        }
    }
}
