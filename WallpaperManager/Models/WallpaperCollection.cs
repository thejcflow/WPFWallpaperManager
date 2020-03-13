using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperManager.Models
{
    public class WallpaperCollection : ObservableCollection<Wallpaper>
    {
        public Wallpaper Next
        {
            get
            {
                if (CurrentIndex >= this.Count || CurrentIndex < 0)
                {
                    CurrentIndex = 0;
                }
                var current = this.Count > 0 ? this[CurrentIndex] : null;
                CurrentIndex++;
                return current;
            }
        }

        public int CurrentIndex { get;  set; }
    }
}
