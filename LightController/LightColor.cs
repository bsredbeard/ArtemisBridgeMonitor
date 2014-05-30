using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Community.BridgeMonitor.LightController
{
    [Flags]
    internal enum LightColor
    {
        Blue = 0x1, Green=0x2, Yellow=0x4, Red=0x8
    }
}
