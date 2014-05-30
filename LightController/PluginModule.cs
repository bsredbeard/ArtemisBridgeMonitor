using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Ninject.Modules;
using Artemis.Community.BridgeMonitor.API;


namespace Artemis.Community.BridgeMonitor.LightController
{
    /// <summary>
    /// This class sets up the Dependency Injection that will expose this plugin in the Bridge Monitor
    /// </summary>
    public class PluginModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBridgePlugin>().To<Config>();
        }
    }
}
