using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WallpaperManager.Controllers;
using System.Windows.Threading;
using ExceptionCore;

namespace WallpaperManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainController = new MainController();
            mainController.Start();

            Dispatcher.UnhandledException += OnException;
        }

        private void OnException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            AbstractException current;
            if (e.Exception is AbstractException)
            {
                current = e.Exception as AbstractException;
            }
            else
            {
                current = new Critical(e.Exception);
            }
            current.LogError();
            MessageBox.Show(current.FriendlyMessage,
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            e.Handled = true;
        }
    }
}
