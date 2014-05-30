using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Community.BridgeMonitor
{
    /// <summary>Defines a ConnectionFailed event for when a connection could not be established</summary>
    interface IConnectionFailed
    {
        /// <summary>The requested connection could not be made</summary>
        void ConnectionFailed();
    }
}
