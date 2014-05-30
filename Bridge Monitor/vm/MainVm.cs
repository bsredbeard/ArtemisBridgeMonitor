using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Artemis.Community.BridgeMonitor.API;

namespace Artemis.Community.BridgeMonitor.vm
{
    public class MainVm : ViewModelBase, IArtemisEventListener, IConnectionFailed, IDisposable
    {
        //private ClientController _clientController = null;
        private bool _disposed = false;
        private bool _disposing = false;
        private System.Windows.Threading.Dispatcher _dispatcher = null;

        public MainVm() : base()
        {
            _dispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;

            PopulateInternalDependencies();
            PropertyChanged += Internal_PropertyChanged;

            if (App.Instance != null)
            {
                App.Instance.RegisterArtemisListener(this);
            }
        }

        ~MainVm()
        {
            ((IDisposable)this).Dispose();
        }

        #region fields
        private string _serverHost = "127.0.0.1";
        private int _serverPort = 2010;

        private Options.ConnectionState _connectionStatus = Options.ConnectionState.Disconnected;
        private ObservableCollection<string> _shipList = new ObservableCollection<string>(new[] { "No ships..." });
        private bool _missionInProgress = false;
        private bool _shieldsActive = false;
        private bool _redAlertActive = false;
        private bool _jumpActive = false;
        private bool _reverseActive = false;
        private int _warpSpeed = 0;
        private int _impulseSpeed = 0;
        private int _selectedShipIndex = 0;
        private double _frontShieldPct = 1;
        private double _rearShieldPct = 1;
        #endregion

        #region observable properties

        [ReliesOn("ConnectionStatus")]
        public string Title
        {
            get
            {
                switch (ConnectionStatus)
                {
                    case Options.ConnectionState.Disconnected: return "Artemis Lighting (Disconnected)";
                    case Options.ConnectionState.Connecting: return "Artemis Lighting (Connecting...)";
                }
                return "Artemis Lighting (Connected)";
            }
        }

        public string ServerHost
        {
            get { return _serverHost; }
            set
            {
                _serverHost = value;
                RaisePropertyChanged(() => ServerHost);
            }
        }

        public int ServerPort
        {
            get { return _serverPort; }
            set
            {
                _serverPort = value;
                RaisePropertyChanged(() => ServerPort);
            }
        }

        public Options.ConnectionState ConnectionStatus
        {
            get { return _connectionStatus; }
            set
            {
                _connectionStatus = value;
                RaisePropertyChanged(() => ConnectionStatus);
            }
        }

        [ReliesOn("ConnectionStatus")]
        public bool IsConnected
        {
            get { return ConnectionStatus == Options.ConnectionState.Connected; }
        }

        [ReliesOn("ConnectionStatus")]
        public string ConnectButtonText
        {
            get
            {
                switch (ConnectionStatus)
                {
                    case Options.ConnectionState.Disconnected: return "Connect";
                    case Options.ConnectionState.Connecting: return "Connecting...";
                }
                return "Disconnect";
            }
        }

        [ReliesOn("ConnectionStatus")]
        public bool CanModifyConnection
        {
            get
            {
                return ConnectionStatus == Options.ConnectionState.Disconnected;
            }
        }

        public ObservableCollection<string> ShipList
        {
            get { return _shipList; }
        }

        public int SelectedShipIndex
        {
            get { return _selectedShipIndex; }
            set
            {
                _selectedShipIndex = value;
                RaisePropertyChanged(() => SelectedShipIndex);
            }
        }

        [ReliesOn("ConnectionStatus")]
        public bool MissionInProgress
        {
            get { return ConnectionStatus == Options.ConnectionState.Connected && _missionInProgress; }
            set
            {
                _missionInProgress = value;
                RaisePropertyChanged(() => MissionInProgress);
            }
        }

        public bool ShieldsActive
        {
            get { return _shieldsActive; }
            set
            {
                _shieldsActive = value;
                RaisePropertyChanged(() => ShieldsActive);
            }
        }

        public bool RedAlertActive
        {
            get { return _redAlertActive; }
            set
            {
                _redAlertActive = value;
                RaisePropertyChanged(() => RedAlertActive);
            }
        }

        public bool ReverseActive
        {
            get { return _reverseActive; }
            set
            {
                _reverseActive = value;
                RaisePropertyChanged(() => ReverseActive);
            }
        }

        public int ImpulseSpeed
        {
            get { return _impulseSpeed; }
            set
            {
                _impulseSpeed = value;
                RaisePropertyChanged(() => ImpulseSpeed);
            }
        }

        [ReliesOn("ImpulseSpeed")]
        public bool ImpulseActive
        {
            get { return ImpulseSpeed > 0; }
        }

        public int WarpSpeed
        {
            get { return _warpSpeed; }
            set
            {
                _warpSpeed = value;
                RaisePropertyChanged(() => WarpSpeed);
            }
        }

        [ReliesOn("WarpSpeed")]
        public bool WarpActive
        {
            get { return WarpSpeed > 0; }
        }

        public bool JumpActive
        {
            get { return _jumpActive; }
            set
            {
                _jumpActive = value;
                RaisePropertyChanged(() => JumpActive);
            }
        }

        public double FrontShieldPct
        {
            get { return _frontShieldPct; }
            set
            {
                _frontShieldPct = value;
                RaisePropertyChanged(() => FrontShieldPct);
            }
        }

        public double RearShieldPct
        {
            get { return _rearShieldPct; }
            set
            {
                _rearShieldPct = value;
                RaisePropertyChanged(() => RearShieldPct);
            }
        }
        #endregion

        #region commands
        private RelayCommand _connect = null;
        public RelayCommand Connect
        {
            get
            {
                if (_connect == null)
                {
                    _connect = new RelayCommand(() =>
                        {
                            if (ConnectionStatus == Options.ConnectionState.Disconnected)
                            {
                                ConnectionStatus = Options.ConnectionState.Connecting;
                                App.Instance.Connect(ServerHost, ServerPort);
                            }
                            else
                            {
                                App.Instance.Disconnect();
                            }
                        },
                        () => ConnectionStatus != Options.ConnectionState.Connecting
                    );
                }
                return _connect;
            }
        }

        private RelayCommand _setShip = null;
        public RelayCommand SetShip
        {
            get
            {
                if (_setShip == null)
                {
                    _setShip = new RelayCommand(() =>
                    {
                        if (IsConnected)
                        {
                            App.Instance.SelectShip(SelectedShipIndex);
                            MissionInProgress = true;
                        }
                        else
                        {
                            MessageBox.Show("you're not connected");
                        }
                    });
                }
                return _setShip;
            }
        }
        #endregion


        #region infrastructure
        private Dictionary<string, List<string>> _internalDependencies = new Dictionary<string, List<string>>();

        private void PopulateInternalDependencies()
        {
            GetType().GetProperties()
                     .SelectMany(p => p.GetCustomAttributes(typeof(ReliesOnAttribute), true).OfType<ReliesOnAttribute>(),
                                 (p, attr) => new { dependent = p.Name, attr = attr })
                     .Where(p => p.attr != null)
                     .ToList().ForEach(p =>
                     {
                         if (!_internalDependencies.ContainsKey(p.attr.Parent))
                         {
                             _internalDependencies[p.attr.Parent] = new List<string>();
                         }
                         _internalDependencies[p.attr.Parent].Add(p.dependent);
                     });
        }

        private void Internal_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_internalDependencies.ContainsKey(e.PropertyName))
            {
                _internalDependencies[e.PropertyName].ForEach(RaisePropertyChanged);
            }
        }
        #endregion

        void IDisposable.Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _disposing = true;
                _disposing = false;
            }
        }

        public void Connected()
        {
            System.Diagnostics.Debug.WriteLine("ConnectedEvent");
            ConnectionStatus = Options.ConnectionState.Connected;
        }

        public void ConnectionLost(bool badDisconnect)
        {
            ConnectionStatus = Options.ConnectionState.Disconnected;
            if (!_disposing)
            {
                if (badDisconnect)
                {
                    _dispatcher.Invoke(new Action(() =>
                    {
                        ConnectionStatus = Options.ConnectionState.Disconnected;
                        MessageBox.Show("The connection to the server has dropped.", "Connection Dropped", MessageBoxButton.OK, MessageBoxImage.Error);
                    }));
                }
            }
        }

        public void GameEnded()
        {
            MissionInProgress = false;
        }

        public void Shields(bool active)
        {
            ShieldsActive = active;
        }

        public void RedAlert(bool active)
        {
            RedAlertActive = active;
        }

        public void ReverseEngines(bool active)
        {
            ReverseActive = active;
        }

        public void ShipNames(string[] shipNames)
        {
            _dispatcher.Invoke(new Action<string[]>((ships) =>
            {
                var tmpSelectedIndex = SelectedShipIndex;
                var jk = 0;
                while (jk < ships.Length)
                {
                    if (ShipList.Count <= jk)
                    {
                        ShipList.Add(ships[jk]);
                    }
                    else
                    {
                        ShipList[jk] = ships[jk];
                    }
                    jk++;
                }
                //trim down the other array to make sure it stays in sync
                while (jk < ShipList.Count)
                {
                    ShipList.RemoveAt(ShipList.Count - 1);
                }
                SelectedShipIndex = tmpSelectedIndex;
            }), new object[]{ (string[])shipNames });
        }

        public void JumpCountdownBegin()
        {
            JumpActive = true;
        }

        public void JumpInitiated()
        {
            JumpActive = false;
        }

        public void ReceiveMessage(string from, string message, int priority)
        {
            System.Diagnostics.Debug.WriteLine("==Incoming transmission==");
            System.Diagnostics.Debug.WriteLine("=From: " + from);
            System.Diagnostics.Debug.WriteLine("---Message---------------");
            System.Diagnostics.Debug.WriteLine(message);
            System.Diagnostics.Debug.WriteLine("==End transmission==");
        }

        public void Damaged()
        {
            System.Diagnostics.Debug.WriteLine("Received damage");
        }

        public void Warp(int factor)
        {
            if (factor >= 0)
            {
                WarpSpeed = factor;
            }
        }

        public void Impulse(double pct)
        {
            if (pct >= 0)
            {
                ImpulseSpeed = (int)(pct * 100);
            }
        }

        public void DamConMemberDeath()
        {
            System.Diagnostics.Debug.WriteLine("He's DEAD, Jim!");
        }

        public void FrontShield(double strength, double max)
        {
            FrontShieldPct = strength / max;
        }

        public void RearShield(double strength, double max)
        {
            RearShieldPct = strength / max;
        }

        public void ConnectionFailed()
        {
            _dispatcher.Invoke(new Action(() =>
            {
                MessageBox.Show("Could not connect to the server. Check your settings and try again.", "Connection Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }));
        }
    }
}
