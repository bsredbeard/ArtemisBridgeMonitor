using StageKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Community.BridgeMonitor.LightController
{
    internal class ControllerWrapper : IDisposable
    {
        private LedDisplay _ledState = null;
        private StageKitController _controller = null;

        public ControllerWrapper(int controllerNumber)
        {
            _ledState = new LedDisplay();
            _controller = new StageKitController(controllerNumber);
        }

        private bool match(LightColor candidate, LightColor test)
        {
            return (candidate & test) == test;
        }

        private bool match(LightIndex candidate, LightIndex test)
        {
            return (candidate & test) == test;
        }

        /// <summary>
        /// Set the specified lights on/off
        /// </summary>
        /// <param name="color">The flagged colors to modify</param>
        /// <param name="num">The light number</param>
        /// <param name="on">The state of the light</param>
        public void SetLight(LightColor color, int num, bool on)
        {
            if (match(color, LightColor.Blue))
            {
                switch (num)
                {
                    case 0: _controller.DisplayBlueLed1(ref _ledState, on); break;
                    case 1: _controller.DisplayBlueLed2(ref _ledState, on); break;
                    case 2: _controller.DisplayBlueLed3(ref _ledState, on); break;
                    case 3: _controller.DisplayBlueLed4(ref _ledState, on); break;
                    case 4: _controller.DisplayBlueLed5(ref _ledState, on); break;
                    case 5: _controller.DisplayBlueLed6(ref _ledState, on); break;
                    case 6: _controller.DisplayBlueLed7(ref _ledState, on); break;
                    case 7: _controller.DisplayBlueLed8(ref _ledState, on); break;
                }
            }
            if (match(color, LightColor.Green))
            {
                switch (num)
                {
                    case 0: _controller.DisplayGreenLed1(ref _ledState, on); break;
                    case 1: _controller.DisplayGreenLed2(ref _ledState, on); break;
                    case 2: _controller.DisplayGreenLed3(ref _ledState, on); break;
                    case 3: _controller.DisplayGreenLed4(ref _ledState, on); break;
                    case 4: _controller.DisplayGreenLed5(ref _ledState, on); break;
                    case 5: _controller.DisplayGreenLed6(ref _ledState, on); break;
                    case 6: _controller.DisplayGreenLed7(ref _ledState, on); break;
                    case 7: _controller.DisplayGreenLed8(ref _ledState, on); break;
                }
            }
            if (match(color, LightColor.Yellow))
            {
                switch (num)
                {
                    case 0: _controller.DisplayYellowLed1(ref _ledState, on); break;
                    case 1: _controller.DisplayYellowLed2(ref _ledState, on); break;
                    case 2: _controller.DisplayYellowLed3(ref _ledState, on); break;
                    case 3: _controller.DisplayYellowLed4(ref _ledState, on); break;
                    case 4: _controller.DisplayYellowLed5(ref _ledState, on); break;
                    case 5: _controller.DisplayYellowLed6(ref _ledState, on); break;
                    case 6: _controller.DisplayYellowLed7(ref _ledState, on); break;
                    case 7: _controller.DisplayYellowLed8(ref _ledState, on); break;
                }
            }
            if (match(color, LightColor.Red))
            {
                switch (num)
                {
                    case 0: _controller.DisplayRedLed1(ref _ledState, on); break;
                    case 1: _controller.DisplayRedLed2(ref _ledState, on); break;
                    case 2: _controller.DisplayRedLed3(ref _ledState, on); break;
                    case 3: _controller.DisplayRedLed4(ref _ledState, on); break;
                    case 4: _controller.DisplayRedLed5(ref _ledState, on); break;
                    case 5: _controller.DisplayRedLed6(ref _ledState, on); break;
                    case 6: _controller.DisplayRedLed7(ref _ledState, on); break;
                    case 7: _controller.DisplayRedLed8(ref _ledState, on); break;
                }
            }
        }

        public void SetLight(LightColor color, LightIndex idx, bool on)
        {
            if (match(idx, LightIndex.One)) SetLight(color, 0, on);
            if (match(idx, LightIndex.Two)) SetLight(color, 1, on);
            if (match(idx, LightIndex.Three)) SetLight(color, 2, on);
            if (match(idx, LightIndex.Four)) SetLight(color, 3, on);
            if (match(idx, LightIndex.Five)) SetLight(color, 4, on);
            if (match(idx, LightIndex.Six)) SetLight(color, 5, on);
            if (match(idx, LightIndex.Seven)) SetLight(color, 6, on);
            if (match(idx, LightIndex.Eight)) SetLight(color, 7, on);
        }

        /// <summary>
        /// Turns an entire ring of lights on and off
        /// </summary>
        /// <param name="color">The flagged colors to modify</param>
        /// <param name="on">The state of the lights</param>
        public void SetLight(LightColor color, bool on)
        {
            if (match(color, LightColor.Blue))
            {
                _controller.DisplayBlueAll(ref _ledState, on);
            }
            if (match(color, LightColor.Green))
            {
                _controller.DisplayGreenAll(ref _ledState, on);
            }
            if (match(color, LightColor.Yellow))
            {
                _controller.DisplayYellowAll(ref _ledState, on);
            }
            if (match(color, LightColor.Red))
            {
                _controller.DisplayRedAll(ref _ledState, on);
            }
        }

        void IDisposable.Dispose()
        {
            if (_controller != null)
            {
                _controller.TurnAllOff();
                _controller = null;
            }
        }

        public void StrobeOn(StrobeSpeed strobeSpeed)
        {
            _controller.TurnStrobeOn(strobeSpeed);
        }

        public void StrobeOff()
        {
            _controller.TurnStrobeOff();
        }
    }
}
