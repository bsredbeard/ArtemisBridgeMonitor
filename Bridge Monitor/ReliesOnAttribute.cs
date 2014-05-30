using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Community.BridgeMonitor
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=true, Inherited=true)]
    public class ReliesOnAttribute : Attribute
    {
        public ReliesOnAttribute(string parent) : base()
        {
            Parent = parent;
        }

        public string Parent { get; private set; }
    }
}
