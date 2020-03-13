using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperManager.Models;

namespace WallpaperManager.DataHandlers
{
    public interface IWallpaperDAO
    {
        List<Wallpaper> Read();
        bool Create(Wallpaper wallpaper);
        bool Delete(int wallpaperId);
        bool Update(int wallpaperId, Wallpaper wallpaper);
    }
}
