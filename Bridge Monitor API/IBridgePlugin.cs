using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace Artemis.Community.BridgeMonitor.API
{
    /// <summary>
    /// Defines a Bridge Plugin, which should be a WPF UserControl object. This user control
    /// will be discovered via Ninject (and must be bound to the IBridgePlugin interface in
    /// your Ninject module) and instantiated when the plugin is automatically loaded by the 
    /// bridge monitor.
    /// </summary>
    public interface IBridgePlugin : IAddChild, IDisposable
    {
        /// <summary>
        /// Get information about the plugin so it can be shown in the about dialog
        /// </summary>
        BridgePluginInfo PluginInfo { get; }

        /// <summary>
        /// The instance of the IArtemisEventListener that this plugin will use to listen for events
        /// </summary>
        IArtemisEventListener BridgeEventListener { get; }
    }
}
