using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Community.BridgeMonitor.API
{
    /// <summary>
    /// Defines a number of method calls that will be triggered by various events in the active game of Artemis
    /// </summary>
    public interface IArtemisEventListener
    {
        /// <summary>
        /// Notification when the client is fully connected.
        /// Not really necessary but might give you the warm fuzzies
        /// </summary>
        void Connected();

        /// <summary>The connection to the server has dropped</summary>
        /// <param name="badDisconnect">true if the disconnect was not planned</param>
        void ConnectionLost(bool badDisconnect);

        /// <summary>Final scoreboard has been displayed, and server has clicked "End Game" button</summary>
        void GameEnded();

        /// <summary>Helm or Weapons has toggled the shields</summary>
        /// <param name="active">true if shields are currently active</param>
        void Shields(bool active);

        /// <summary>Comms has toggled red alert</summary>
        /// <param name="active">true if red alert is currently active</param>
        void RedAlert(bool active);

        /// <summary>Helm has toggled the engine reverse control</summary>
        /// <param name="active">true if reverse is currently active</param>
        void ReverseEngines(bool active);

        /// <summary>The list of ship names has been sent from the server</summary>
        /// <param name="shipNames">The array of the ship names</param>
        void ShipNames(string[] shipNames);

        /// <summary>Helm has initiated a jump countdown</summary>
        void JumpCountdownBegin();

        /// <summary>The ship has begun the actual jump to its target location</summary>
        void JumpInitiated();

        /// <summary>
        /// Received a comms message
        /// </summary>
        /// <param name="from">the name of the entity that sent it</param>
        /// <param name="message">the message itself</param>
        /// <param name="priority">the priority broadcast with the message</param>
        void ReceiveMessage(string from, string message, int priority);

        /// <summary>The ship took damage to one of its sectors</summary>
        void Damaged();

        /// <summary>The warp factor has been changed</summary>
        /// <param name="factor">the new warp factor</param>
        void Warp(int factor);

        /// <summary>The impulse speed has been changed</summary>
        /// <param name="pct">the impulse speed, scale of 0-1</param>
        void Impulse(double pct);

        /// <summary>A member of one of the DamCon teams has died</summary>
        void DamConMemberDeath();

        /// <summary>The front shield's effectiveness has changed</summary>
        /// <param name="strength">the current strength of the shield</param>
        /// <param name="max">the max strength of the shield</param>
        void FrontShield(double strength, double max);

        /// <summary>The rear shield's effectiveness has changed</summary>
        /// <param name="strength">the current strength of the shield</param>
        /// <param name="max">the max strength of the shield</param>
        void RearShield(double strength, double max);
    }
}
