using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Artemis.Community.BridgeMonitor
{
    /// <summary>
    /// Performs tasks relating to discovering the plugins
    /// </summary>
    static class PluginDiscovery
    {
        /// <summary>
        /// Loads plugins into the application by searching the adjacent "plugins" directory.
        /// Each folder inside the plugins directory has its full path + "*.dll" added to the
        /// assembly search path, then ninject is told to scan all of those DLLs for modules.
        /// </summary>
        /// <param name="kernel">The ninject kernel instance to load plugins into</param>
        public static void ScanPluginFolder(IKernel kernel)
        {
            var pluginFolder = new DirectoryInfo("plugins");
            var pluginPaths = new List<string>();
            if (pluginFolder.Exists)
            {
                pluginPaths.AddRange(
                    pluginFolder.GetDirectories()
                                .Select(d => Path.Combine(d.FullName, "*.dll"))
                );
                kernel.Load(pluginPaths.ToArray());
            }
        }

    }
}
