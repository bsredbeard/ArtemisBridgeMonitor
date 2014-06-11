using Artemis.Community.BridgeMonitor.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ninject;

namespace Artemis.Community.BridgeMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<PluginWindow> _pluginWindows = new List<PluginWindow>();

        public MainWindow()
        {
            InitializeComponent();

            //When the main window has been shown and rendered, load up the plugins
            this.ContentRendered += (s, e) =>
            {
                var app = App.Instance;

                foreach (var plug in app.Kernel.GetAll<IBridgePlugin>())
                {
                    var plugin = (UserControl)plug;
                    var wind = new PluginWindow(plugin, plug.PluginInfo);
                    wind.Show();

                    app.RegisterArtemisListener(plug.BridgeEventListener);

                    _pluginWindows.Add(wind);
                }

            };

            //When closing the main window, collect all the plugin windows
            this.Closed += (s, e) =>
            {
                foreach (var wind in _pluginWindows)
                {
                    wind.Close();
                }
                _pluginWindows.RemoveRange(0, _pluginWindows.Count);
                _pluginWindows = null;

                if (this.DataContext is IDisposable)
                {
                    ((IDisposable)this.DataContext).Dispose();
                }
            };
        }
    }
}
