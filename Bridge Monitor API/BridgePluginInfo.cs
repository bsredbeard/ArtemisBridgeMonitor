using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Community.BridgeMonitor.API
{
    /// <summary>
    /// Defines information about the plugin. This struct is required by the IBridgePlugin interface 
    /// </summary>
    public struct BridgePluginInfo
    {
        /// <summary>
        /// The name of the plugin, required, will be displayed
        /// </summary>
        public string PluginName { get; set; }

        /// <summary>
        /// The version of the plugin, will be displayed
        /// </summary>
        public Version PluginVersion { get; set; }

        /// <summary>
        /// The name the author, optional, will be displayed if provided
        /// </summary>
        public string PluginAuthor { get; set; }

        /// <summary>
        /// The url to the plugin's page, optional, will be displayed and clickable if provided
        /// </summary>
        public Uri PluginUrl { get; set; }

        /// <summary>
        /// The url to the plugin's license, optional, will be displayed and clickable if provided
        /// </summary>
        public Uri PluginLicenseUrl { get; set; }
    }
}
