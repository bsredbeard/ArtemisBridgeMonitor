using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Community.BridgeMonitor.LightController
{
    [Flags]
    internal enum LightIndex
    {
        One = 1,
        Two = 2,
        Three = 4,
        Four = 8,
        Five = 16,
        Six = 32,
        Seven = 64,
        Eight = 128
    }
}
