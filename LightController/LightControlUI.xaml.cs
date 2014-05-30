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

namespace Artemis.Community.BridgeMonitor.LightController
{
    /// <summary>
    /// Interaction logic for Config.xaml
    /// </summary>
    public partial class Config : UserControl, IBridgePlugin
    {
        private SkListener _listener = new SkListener();

        public Config()
        {
            InitializeComponent();
        }

        public BridgePluginInfo PluginInfo
        {
            get
            {
                return new BridgePluginInfo()
                {
                    PluginName = "Stage Kit Light Controller",
                    PluginUrl = new Uri("http://www.mentalspike.com"),
                    PluginAuthor = "Bill Smith",
                    PluginVersion =  new Version(0, 1)
                };
            }
        }

        public void Dispose()
        {
            if (_listener != null)
            {
                lock (_listener)
                {
                    if (_listener != null)
                    {
                        ((IDisposable)_listener).Dispose();
                        _listener = null;
                    }
                }
            }
        }


        IArtemisEventListener IBridgePlugin.BridgeEventListener
        {
            get { return _listener; }
        }
    }
}
