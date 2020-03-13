using ExceptionCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WallpaperManager.DataHandlers;
using WallpaperManager.Models;
using WallpaperManager.Utils;
using WallpaperManager.Views;

namespace WallpaperManager.Controllers
{
    public class MainController
    {
        private const int MINUTE = 5000;
        private const int MAX_COLLECTION_SIZE = 50;
        private const int MAX_FILE_SIZE = 2621440;
        public int RefreshTime
        {
            get { return delayInterval; }
        }

        private MainWindow window;
        private WallpaperCollection collection;
        private IWallpaperSetter wallpaperSetter;
        private int delayInterval;
        private Timer timer;
        private IWallpaperDAO wallpaperDAO;

        public MainController()
        {
            window = new MainWindow(this);
            collection = new WallpaperCollection();
            collection.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(window.CollectionChanged);
            wallpaperSetter = new WindowsWallpaperSetter();
            wallpaperDAO = new SQLWallpaperDAO();
            delayInterval = 1;
        }

        public void Start()
        {
            wallpaperSetter.StyleType = WallpaperStyleType.Centered;
            RefreshItems();
            timer = new Timer(delayInterval * MINUTE);
            timer.Elapsed += OnTimedEvent;
            PaintWallpaper();
            ResetTimer();
            window.Initialize();
            window.Show();
        }


        public void UpdateWallpaperStyle(WallpaperStyleType styleType)
        {
            wallpaperSetter.StyleType = styleType;
            collection.CurrentIndex = collection.CurrentIndex - 1;
            ResetTimer();
            PaintWallpaper();
        }

        public void UpdateDelayMinutes(int minutes)
        {
            delayInterval = minutes;
            ResetTimer();
        }

        public void AddWallpaper(string path)
        {

            if (!File.Exists(path))
            {
                return;
            }
            if (collection.Count >= MAX_COLLECTION_SIZE)
            {
                throw new CollectionLimitExceededException();
            }
            if (new FileInfo(path).Length > MAX_FILE_SIZE)
            {
                throw new WallpaperSizeException();
            }
            var wallpaper = new Wallpaper()
            {
                Name = Path.GetFileName(path),
                Image = ImageFileHelper.ReadFile(path)
            };

            var created = wallpaperDAO.Create(wallpaper);
            if (created)
            {
                RefreshItems();

                if (collection.Count == 1)
                {
                    ResetTimer();
                    PaintWallpaper();
                }
            }
        }

        public List<string> GetImagePaths()
        {
            var result = new List<string>();
            foreach (var wallpaper in collection)
            {
                var path = ImageFileHelper.GetFilePath(wallpaper);
                result.Add(path);
            }
            return result;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            PaintWallpaper();
        }

        private void PaintWallpaper()
        {
            if (collection.Count <= 0)
            {
                return;
            }
            var wallpaper = collection.Next;
            wallpaperSetter.Path = ImageFileHelper.GetFilePath(wallpaper);
            if (wallpaperSetter.Path != null && File.Exists(wallpaperSetter.Path))
            {
                wallpaperSetter.Paint();
            }
        }

        public void UpdateCurrentIndex(int index)
        {
            collection.CurrentIndex = index;
            ResetTimer();
            PaintWallpaper();
        }

        public void RemoveWallpaper(int selectedIndex)
        {
            var wallpaper = collection[selectedIndex];
            var deleted = wallpaperDAO.Delete(wallpaper.Id);
            if (deleted)
            {
                RefreshItems();
                PaintWallpaper();
                ResetTimer();
            }
        }

        private void ResetTimer()
        {
            timer.AutoReset = true;
            timer.Enabled = false;
            timer.Enabled = true;
        }

        private void RefreshItems()
        {
            var wallpapers = wallpaperDAO.Read();
            collection.Clear();
            foreach (var wallpaper in wallpapers)
            {
                ImageFileHelper.WriteFile(wallpaper.Image, wallpaper);
                collection.Add(wallpaper);
            }
        }
    }
}
