using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Community.BridgeMonitor.Design
{
    /// <summary>
    /// Mimics the PluginInfo struct for easier design-time binding
    /// </summary>
    internal struct PluginInfoDesignProxy
    {
        /// <summary>
        /// The name of the plugin, required, will be displayed
        /// </summary>
        public string PluginName { get { return "Fancy Plugin"; } set { } }

        /// <summary>
        /// The version of the plugin, will be displayed
        /// </summary>
        public Version PluginVersion { get { return new Version(1, 5); } set { } }

        /// <summary>
        /// The name the author, optional, will be displayed if provided
        /// </summary>
        public string PluginAuthor { get { return "Some Guy!"; } set { } }

        /// <summary>
        /// The url to the plugin's page, optional, will be displayed and clickable if provided
        /// </summary>
        public Uri PluginUrl { get { return new Uri("http://example.com"); } set { } }

        /// <summary>
        /// The url to the plugin's license, optional, will be displayed and clickable if provided
        /// </summary>
        public Uri PluginLicenseUrl { get { return null; } set { } }
    }
}
