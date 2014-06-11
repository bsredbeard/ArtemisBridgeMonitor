using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Artemis.Community.BridgeMonitor
{
    /// <summary>
    /// Interaction logic for PluginWindow.xaml
    /// </summary>
    public partial class PluginWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public PluginWindow(UserControl ctrl, API.BridgePluginInfo bridgePluginInfo)
        {
            this.DataContext = bridgePluginInfo;

            InitializeComponent();
            pluginHost.Children.Add(ctrl);
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (App.Instance.MainWindow != null && App.Instance.MainWindow.IsVisible)
            {
                e.Cancel = true;
                MessageBox.Show("Plugin windows will be closed when the Bridge Monitor window is closed.");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
    }
}
