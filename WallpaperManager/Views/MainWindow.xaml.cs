using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WallpaperManager.Controllers;
using WallpaperManager.Models;
using WallpaperManager.Utils;

namespace WallpaperManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string LOGO_PATH = "Resources/logo.ico";

        public ObservableCollection<BitmapImage> Images;
        private NotifyIcon tray;
        private MainController controller;
        public MainWindow(MainController controller)
        {
            InitializeComponent();
            this.controller = controller;

            var uri = new Uri(LOGO_PATH, UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(uri);
            tray = new NotifyIcon();
            tray.Icon = new System.Drawing.Icon(LOGO_PATH);
            tray.DoubleClick += delegate (object sender, EventArgs args)
            {
                Show();
                WindowState = WindowState.Normal;
            };
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                tray.Visible = true;
                Hide();
            }
            else
            {
                tray.Visible = false;
            }
            base.OnStateChanged(e);
        }

        public void Initialize()
        {
            wallpapersListBox.ItemsSource = Images;

            styleComboBox.Items.Add("Centered");
            styleComboBox.Items.Add("Stretched");
            styleComboBox.Items.Add("Tiled");
            refreshTextBox.Text = controller.RefreshTime.ToString();
        }
        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Images = new ObservableCollection<BitmapImage>();
            foreach (var wallpaperPath in controller.GetImagePaths())
            {
                var image = new BitmapImage(new Uri(wallpaperPath));
                Images.Add(image);
            }
            wallpapersListBox.ItemsSource = Images;
        }

        public void UpdateSelection(int index)
        {
            wallpapersListBox.SelectedIndex = index;
        }

        private void styleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsVisible)
            {
                return;
            }
            var styleIndex = styleComboBox.SelectedIndex;
            WallpaperStyleType styleType;
            switch (styleIndex)
            {
                default:
                case 0:
                    styleType = WallpaperStyleType.Centered;
                    break;
                case 1:
                    styleType = WallpaperStyleType.Stretched;
                    break;
                case 2:
                    styleType = WallpaperStyleType.Tiled;
                    break;
            }

            controller.UpdateWallpaperStyle(styleType);
        }

        private void refreshTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            int minutes = 0;
            bool converted = int.TryParse(refreshTextBox.Text, out minutes);
            if (!converted || minutes < 1)
            {
                minutes = 1;
            }
            else if (minutes > 999)
            {
                minutes = 999;
            }
            refreshTextBox.Text = minutes.ToString();
            controller.UpdateDelayMinutes(minutes);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Image";
            dlg.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                controller.AddWallpaper(dlg.FileName);
            }
            dlg.Dispose();
        }

        private void setWallpaperClick(object sender, RoutedEventArgs e)
        {
            int selectedIndex = wallpapersListBox.SelectedIndex;
            controller.UpdateCurrentIndex(selectedIndex);
        }

        private void removeWallpaperClick(object sender, RoutedEventArgs e)
        {
            int selectedIndex = wallpapersListBox.SelectedIndex;
            controller.RemoveWallpaper(selectedIndex);
        }
    }
}
