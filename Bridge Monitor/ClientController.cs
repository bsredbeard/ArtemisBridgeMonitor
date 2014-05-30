using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis.Community.BridgeMonitor.API;

namespace Artemis.Community.BridgeMonitor
{
    /// <summary>
    /// Orchestrates communcation between an IMainView and an Artemis server
    /// by way of the ArtemisSBS-ProtocolSharp library
    /// </summary>
    public class ClientController : IDisposable
    {
        private bool _disposed = false;
        private com.artemis.monitor.ArtemisClient _client = new com.artemis.monitor.ArtemisClient();
        private ArtemisEventProxy _proxy = new ArtemisEventProxy();
        private List<IConnectionFailed> _connectionFailedListeners = new List<IConnectionFailed>();

        /// <summary>
        /// create a new controller, optionally registering the first listener
        /// </summary>
        /// <param name="defaultListener">the first listener to add; no registration is performed if null</param>
        public ClientController(IArtemisEventListener defaultListener)
        {
            //add the proxy listener
            _client.AddListener(_proxy);

            //configure the default listener if necessary
            if (defaultListener != null)
            {
                _proxy.RegisterListener(defaultListener);
            }
        }

        ~ClientController()
        {
            ((IDisposable)this).Dispose();
        }

        /// <summary>Registers the provided listener for updates from the artemis client</summary>
        /// <param name="listener">the listener to add</param>
        public void RegisterListener(IArtemisEventListener listener)
        {
            _proxy.RegisterListener(listener);
            if (listener is IConnectionFailed)
            {
                System.Diagnostics.Debug.WriteLine("Adding Connection Failed listener");
                _connectionFailedListeners.Add((IConnectionFailed)listener);
            }
        }

        /// <summary>Removes the provided listener from the artemis client</summary>
        /// <param name="listener">the listener to remove</param>
        public void RemoveListener(IArtemisEventListener listener)
        {
            _proxy.RemoveListener(listener);
        }

        /// <summary>
        /// Returns true if Connect has been called and the connection has not
        /// been reset by a call to Disconnect or some sort of connection failure
        /// </summary>
        public bool IsConnected { get { return _client.IsConnected(); } }

        /// <summary>Initiates a connection to the requested server.</summary>
        public void Connect(string host, int port)
        {
            if (!IsConnected)
            {
                if (!_client.Connect(host, port))
                {
                    _connectionFailedListeners.ForEach(l => l.ConnectionFailed());
                }
            }
            else
            {
                throw new InvalidOperationException("Server connection object already exists. Check IsConnected property before calling Connect.");
            }
        }

        /// <summary>Disconnects the Artemis Client</summary>
        public void Disconnect()
        {
            if (_client != null)
            {
                _client.Disconnect();
            }
        }


        /// <summary>Choose the specified ship and select the observer station</summary>
        /// <param name="shipIndex">The index of the ship to select</param>
        public void SelectShip(int shipIndex)
        {
            if (_client.IsConnected())
            {
                _client.SetShip(shipIndex);
            }
        }


        #region IDisposable members
        void IDisposable.Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                if (_client != null)
                {
                    _client.Disconnect();
                    _client.RemoveListener(_proxy);
                    _client = null;
                }

                if (_proxy != null)
                {
                    ((IDisposable)_proxy).Dispose();
                    _proxy = null;
                }

                _connectionFailedListeners = null;
            }
        }
        #endregion

        /// <summary>
        /// Provides a simple dispersion interface for client events
        /// </summary>
        private class ArtemisEventProxy : IArtemisEventListener, com.artemis.monitor.IArtemisEventListener, IDisposable
        {
            private bool _disposed = false;
            private object _modifyLock = new object();
            private List<IArtemisEventListener> _listeners = new List<IArtemisEventListener>();

            /// <summary>
            /// Add the specified listener to the proxy
            /// </summary>
            /// <param name="listener">the listener to add</param>
            public void RegisterListener(IArtemisEventListener listener)
            {
                if (listener != null)
                {
                    lock (_modifyLock)
                    {
                        if (!_listeners.Contains(listener))
                        {
                            _listeners.Add(listener);
                        }
                    }
                }
            }

            /// <summary>
            /// Remove the specified listener from the array
            /// </summary>
            /// <param name="listener"></param>
            public void RemoveListener(IArtemisEventListener listener)
            {
                lock (_modifyLock)
                {
                    while (_listeners.Remove(listener)) ;
                }
            }

            #region IArtemisEventListener members
            public void Connected()
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.Connected());
                }
            }

            public void ConnectionLost(bool badDisconnect)
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.ConnectionLost(badDisconnect));
                }
            }

            public void GameEnded()
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.GameEnded());
                }
            }

            public void Shields(bool active)
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.Shields(active));
                }
            }

            public void RedAlert(bool active)
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.RedAlert(active));
                }
            }

            public void ReverseEngines(bool active)
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.ReverseEngines(active));
                }
            }

            public void ShipNames(string[] shipNames)
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.ShipNames(shipNames));
                }
            }

            public void JumpCountdownBegin()
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.JumpCountdownBegin());
                }
            }

            public void JumpInitiated()
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.JumpInitiated());
                }
            }

            public void ReceiveMessage(string from, string message, int priority)
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.ReceiveMessage(from, message, priority));
                }
            }

            public void Damaged()
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.Damaged());
                }
            }

            public void Warp(int factor)
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.Warp(factor));
                }
            }

            public void Impulse(double pct)
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.Impulse(pct));
                }
            }

            public void DamConMemberDeath()
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.DamConMemberDeath());
                }
            }

            public void FrontShield(double strength, double max)
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.FrontShield(strength, max));
                }
            }

            public void RearShield(double strength, double max)
            {
                lock (_modifyLock)
                {
                    _listeners.ForEach(l => l.RearShield(strength, max));
                }
            }
            #endregion

            void IDisposable.Dispose()
            {
                if (!_disposed)
                {
                    _disposed = true;
                    lock (_modifyLock)
                    {
                        while (_listeners.Count > 0)
                        {
                            _listeners.RemoveAt(_listeners.Count - 1);
                        }
                    }
                }
            }
        }
    }
}
