using Artemis.Community.BridgeMonitor.API;
using StageKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artemis.Community.BridgeMonitor.LightController
{
    public class SkListener : IArtemisEventListener, IDisposable
    {
        ControllerWrapper _wrapper = null;

        public SkListener(int controllerPort = 1)
        {
            _wrapper = new ControllerWrapper(controllerPort);
        }

        public void Connected()
        {
            new Task(() => ConnectAnimation()).Start();
        }

        public void ConnectionLost(bool badDisconnect)
        {
            
        }

        public void GameEnded()
        {
            
        }

        public void Shields(bool active)
        {
            _wrapper.SetLight(LightColor.Blue, active);
        }

        public void RedAlert(bool active)
        {
            _wrapper.SetLight(LightColor.Red, active);
        }

        public void ReverseEngines(bool active)
        {
            
        }

        public void ShipNames(string[] shipNames) { }

        public void JumpCountdownBegin()
        {
            
        }

        public void JumpInitiated()
        {
            new Task(() => JumpStrobe()).Start();
        }

        public void ReceiveMessage(string from, string message, int priority)
        {
            
        }

        public void Damaged()
        {
            
        }

        public void Warp(int factor)
        {
            
        }

        public void Impulse(double pct)
        {
            
        }

        public void DamConMemberDeath()
        {
            
        }

        public void FrontShield(double strength, double max)
        {
            
        }

        public void RearShield(double strength, double max)
        {
            
        }

        #region animations
        private async void ConnectAnimation(int delay = 250)
        {
            var all = LightColor.Blue | LightColor.Green | LightColor.Red;

            _wrapper.SetLight(all, 0, true);
            await TaskEx.Delay(TimeSpan.FromMilliseconds(delay));
            _wrapper.SetLight(all, 0, false);
            _wrapper.SetLight(all, 1, true);
            await TaskEx.Delay(TimeSpan.FromMilliseconds(delay));
            _wrapper.SetLight(all, 1, false);
            _wrapper.SetLight(all, 2, true);
            await TaskEx.Delay(TimeSpan.FromMilliseconds(delay));
            _wrapper.SetLight(all, 2, false);
            _wrapper.SetLight(all, 3, true);
            await TaskEx.Delay(TimeSpan.FromMilliseconds(delay));
            _wrapper.SetLight(all, 3, false);
            _wrapper.SetLight(all, 4, true);
            await TaskEx.Delay(TimeSpan.FromMilliseconds(delay));
            _wrapper.SetLight(all, 4, false);
            _wrapper.SetLight(all, 5, true);
            await TaskEx.Delay(TimeSpan.FromMilliseconds(delay));
            _wrapper.SetLight(all, 5, false);
            _wrapper.SetLight(all, 6, true);
            await TaskEx.Delay(TimeSpan.FromMilliseconds(delay));
            _wrapper.SetLight(all, 6, false);
            _wrapper.SetLight(all, 7, true);
            await TaskEx.Delay(TimeSpan.FromMilliseconds(delay));
            _wrapper.SetLight(all, 7, false);
        }

        private async void JumpStrobe()
        {
            _wrapper.StrobeOn(StrobeSpeed.Faster);
            await TaskEx.Delay(TimeSpan.FromMilliseconds(500));
            _wrapper.StrobeOn(StrobeSpeed.Medium);
            await TaskEx.Delay(TimeSpan.FromMilliseconds(1500));
            _wrapper.StrobeOn(StrobeSpeed.Slow);
            await TaskEx.Delay(TimeSpan.FromMilliseconds(1500));
            _wrapper.StrobeOff();
        }
        #endregion

        void IDisposable.Dispose()
        {
            if (_wrapper != null)
            {
                var id = _wrapper as IDisposable;
                if (id != null)
                {
                    id.Dispose();
                }
                _wrapper = null;
            }
        }
    }
}
