using Artemis.Community.BridgeMonitor.API;
using Artemis.Community.BridgeMonitor.LightController;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace Artemis.Community.BridgeMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Dispatcher _dispatcher = null;
        private ClientController _clientController = null;
        private SkListener _lightListener = null;

        public App() : base()
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("You may only have one instance of the application. Check static member Instance before instantiating a new copy of the App");
            }
            Instance = this;
            _dispatcher = Dispatcher.CurrentDispatcher;
            _clientController = new ClientController(null);

            _lightListener = new SkListener(1);
            _clientController.RegisterListener(_lightListener);

            this.Exit += (sender, e) =>
            {
                IDisposable id = _clientController as IDisposable;
                if (id != null)
                {
                    id.Dispose();
                }
                _clientController = null;

                id = _lightListener as IDisposable;
                if (id != null)
                {
                    id.Dispose();
                }
                _lightListener = null;
            };
        }

        public static App Instance { get; private set; }

        public void RegisterArtemisListener(IArtemisEventListener listener)
        {
            _dispatcher.Invoke(new Action<IArtemisEventListener>(l =>
            {
                _clientController.RegisterListener(l);
            }), listener);
        }

        public void Connect(string serverHost, int serverPort)
        {
            _dispatcher.Invoke(new Action<string, int>((host, port) =>
            {
                _clientController.Connect(host, port);
            }), serverHost, serverPort);
        }

        public void Disconnect()
        {
            _dispatcher.Invoke(new Action(() => _clientController.Disconnect()));
        }

        public void SelectShip(int shipIndex)
        {
            _dispatcher.Invoke(new Action<int>(si => _clientController.SelectShip(si)), shipIndex);
        }

    }
}
