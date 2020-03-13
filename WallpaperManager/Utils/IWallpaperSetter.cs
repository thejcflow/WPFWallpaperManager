using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperManager.Models;

namespace WallpaperManager.Utils
{
    public interface IWallpaperSetter
    {
        string Path { get; set; }

        WallpaperStyleType StyleType { get; set; }

        void Paint();
    }
}
