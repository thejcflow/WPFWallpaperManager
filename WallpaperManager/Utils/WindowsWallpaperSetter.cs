using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WallpaperManager.Models;

namespace WallpaperManager.Utils
{
    public class WindowsWallpaperSetter : IWallpaperSetter
    {
        public string Path { get; set; }

        public WallpaperStyleType StyleType { get; set; }

        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public void Paint()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            string wallpaperStyle;
            string tileWallpaper;
            switch (StyleType)
            {
                default:
                case WallpaperStyleType.Stretched:
                    wallpaperStyle = "2";
                    tileWallpaper = "0";
                    break;
                case WallpaperStyleType.Centered:
                    wallpaperStyle = "1";
                    tileWallpaper = "0";
                    break;
                case WallpaperStyleType.Tiled:
                    wallpaperStyle = "1";
                    tileWallpaper = "1";
                    break;
            }

            key.SetValue(@"WallpaperStyle", wallpaperStyle);
            key.SetValue(@"TileWallpaper", tileWallpaper);
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, Path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
