using Artemis.Community.BridgeMonitor.API;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Ninject;

namespace Artemis.Community.BridgeMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Dispatcher _dispatcher = null;
        private ClientController _clientController = null;

        /// <summary>
        /// Access to the static application instance
        /// </summary>
        public static App Instance { get; private set; }

        public App() : base()
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("You may only have one instance of the application. Check static member Instance before instantiating a new copy of the App");
            }
            Instance = this;
            _dispatcher = Dispatcher.CurrentDispatcher;
            _clientController = new ClientController(null);

            Kernel = new StandardKernel();
            PluginDiscovery.ScanPluginFolder(Kernel);

            //bind the application exit event to ensure that all the resources get disposed of
            this.Exit += (sender, e) =>
            {
                IDisposable id = _clientController as IDisposable;
                if (id != null)
                {
                    id.Dispose();
                }
                _clientController = null;
            };
        }

        /// <summary>
        /// Access to the dependency injection kernel
        /// </summary>
        public IKernel Kernel { get; private set; }

        /// <summary>
        /// Registers a listener into the artemis client
        /// </summary>
        /// <param name="listener"></param>
        public void RegisterArtemisListener(IArtemisEventListener listener)
        {
            _dispatcher.Invoke(new Action<IArtemisEventListener>(l =>
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Registering listener of type {0}", l.GetType().FullName));
                _clientController.RegisterListener(l);
            }), listener);
        }

        /// <summary>
        /// Connect to the Artemis server
        /// </summary>
        /// <param name="serverHost">Remote host to connect to</param>
        /// <param name="serverPort">Remote port to connect to</param>
        public void Connect(string serverHost, int serverPort)
        {
            _dispatcher.Invoke(new Action<string, int>((host, port) =>
            {
                _clientController.Connect(host, port);
            }), serverHost, serverPort);
        }

        /// <summary>
        /// Disconnect the client
        /// </summary>
        public void Disconnect()
        {
            _dispatcher.Invoke(new Action(() => _clientController.Disconnect()));
        }

        /// <summary>
        /// Select the given ship to connect this client listener on
        /// </summary>
        /// <param name="shipIndex">the 0-based ship index, up to 7, inclusive</param>
        public void SelectShip(int shipIndex)
        {
            _dispatcher.Invoke(new Action<int>(si => _clientController.SelectShip(si)), shipIndex);
        }

    }
}
