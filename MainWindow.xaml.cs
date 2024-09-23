using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Forms; // For NotifyIcon
using System.Drawing; // For Icon

namespace AIAppLauncher
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<AppIcon> _appIcons;
        private NotifyIcon _notifyIcon;
        private const int HOTKEY_ID = 9000;
        private IntPtr _windowHandle;
        private bool _isFullScreenMode = false;
        private double _buttonSize = 64; // Default size

        public event PropertyChangedEventHandler PropertyChanged;

        public double ButtonSize
        {
            get { return _buttonSize; }
            set
            {
                if (_buttonSize != value)
                {
                    _buttonSize = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonSize)));
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_SHIFT = 0x0004;
        private const uint VK_A = 0x41;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitializeAppIcons();
            InitializeNotifyIcon();
            
            this.KeyDown += MainWindow_KeyDown;
            this.Closing += MainWindow_Closing;
            this.Loaded += MainWindow_Loaded;

            // Hide the window on startup
            //this.Hide();

            ShowWindow();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _windowHandle = new WindowInteropHelper(this).Handle;
            HwndSource source = HwndSource.FromHwnd(_windowHandle);
            source?.AddHook(WndProc);

            RegisterGlobalHotKey();
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                ShowWindow();
                handled = true;
            }
            return IntPtr.Zero;
        }

        private void RegisterGlobalHotKey()
        {
            bool result = RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL | MOD_ALT | MOD_SHIFT, VK_A);
            if (!result)
            {
                System.Windows.MessageBox.Show("Failed to register hotkey. The app may already be running.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeAppIcons()
        {
            _appIcons = new ObservableCollection<AppIcon>
            {
                new AppIcon { Name = "ChatGPT", IconPath = "/Images/chatgpt.png", Action = "https://chatgpt.com/" },
                new AppIcon { Name = "Claude", IconPath = "/Images/claude.png", Action = "https://claude.ai/" },
                new AppIcon { Name = "Perplexity", IconPath = "/Images/perplexity.png", Action = "https://www.perplexity.ai/" },
                new AppIcon { Name = "Bing Copilot", IconPath = "/Images/bingcopilot.png", Action = "https://bing.com/chat" }
                // Add more AI apps here
            };
            AppIconsControl.ItemsSource = _appIcons;
        }

        private void InitializeNotifyIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = new Icon(System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Images/icon.ico")).Stream),
                Visible = true,
                Text = "AI App Launcher"
            };
            _notifyIcon.Click += NotifyIcon_Click;

            // Add a context menu to the notify icon
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Show", null, (s, e) => ShowWindow());
            contextMenu.Items.Add("Exit", null, (s, e) => System.Windows.Application.Current.Shutdown());
            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            ShowWindow();
        }

        private void ShowWindow()
        {
            this.Show();
            if (_isFullScreenMode)
            {
                SetFullScreenMode();
            }
            else
            {
                SetCompactMode();
            }
            this.Activate();
        }

        private void SetFullScreenMode()
        {
            this.WindowState = WindowState.Normal;
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            this.Left = 0;
            this.Top = 0;
            Overlay.Visibility = Visibility.Visible;
            ButtonSize = 64; // Larger buttons in full-screen mode
        }

        private void SetCompactMode()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            this.Width = 266; // Adjust as needed
            this.Height = 64; // Adjust as needed
            this.Left = screenWidth - this.Width - 30; // 30 pixels from right edge
            this.Top = screenHeight - this.Height - 60; // 60 pixels from bottom edge
            Overlay.Visibility = Visibility.Visible;
            ButtonSize = 48; // Smaller buttons in compact mode
        }

        private void HideWindow()
        {
            this.Hide();
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                HideWindow();
            }
            else if (e.Key == Key.Q)
            {
                System.Windows.Application.Current.Shutdown();
            }
            else if (e.Key == Key.T) {
                ToggleMode();
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            HideWindow();
        }

        private void ToggleModeButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleMode();
        }

        private void ToggleMode() {
            _isFullScreenMode = !_isFullScreenMode;
            if (_isFullScreenMode)
            {
                SetFullScreenMode();
            }
            else
            {
                SetCompactMode();
            }
        }

        private void AppIcon_Click(object sender, RoutedEventArgs e)
        {
            var appIcon = (sender as FrameworkElement)?.DataContext as AppIcon;
            if (appIcon != null)
            {
                if (Uri.TryCreate(appIcon.Action, UriKind.Absolute, out Uri uriResult) 
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                {
                    // It's a URL, open in default browser
                    Process.Start(new ProcessStartInfo(appIcon.Action) { UseShellExecute = true });
                }
                else
                {
                    // It's an app, try to launch it
                    try
                    {
                        Process.Start(appIcon.Action);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show($"Failed to launch {appIcon.Name}: {ex.Message}");
                    }
                }
                HideWindow(); // Hide the launcher after launching an app
            }
        }

        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var appIcon = (sender as FrameworkElement)?.DataContext as AppIcon;
            if (appIcon != null)
            {
                //InfoText.Text = appIcon.Name;
            }
        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //InfoText.Text = "Ctrl+Alt+Shift+A to open. Esc to minimize. Q to quit. Toggle for full/compact mode.";
        }

        protected override void OnClosed(EventArgs e)
        {
            _notifyIcon.Dispose();
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
            base.OnClosed(e);
        }
    }

    public class AppIcon : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string IconPath { get; set; }
        public string Action { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}