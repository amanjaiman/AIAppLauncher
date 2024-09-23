using System;
using System.Threading;
using System.Windows;

namespace AIAppLauncher
{
    public partial class App : System.Windows.Application
    {
        private const string MutexName = "AIAppLauncherSingleInstance";
        private Mutex _mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _mutex = new Mutex(true, MutexName, out bool createdNew);

            if (!createdNew)
            {
                // Another instance is already running
                System.Windows.MessageBox.Show("AI App Launcher is already running in the system tray.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                Shutdown();
                return;
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mutex?.ReleaseMutex();
            base.OnExit(e);
        }
    }
}