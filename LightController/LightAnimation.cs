using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Community.BridgeMonitor.LightController
{
    /// <summary>
    /// Defines a light animation that can be run asynchronously
    /// </summary>
    abstract class LightAnimation
    {
        /// <summary>
        /// The colors utilized by this animation
        /// </summary>
        public LightColor Colors { get; protected set; }

        /// <summary>
        /// If true, the animation has an instant duration and will be restored following any other animations that utilize these lights
        /// </summary>
        public bool Persistent { get; protected set; }

        /// <summary>
        /// Run the animation for the given duration
        /// </summary>
        /// <param name="duration">The total length of the animation in milliseconds</param>
        public abstract void Run(int duration);

    }
}
