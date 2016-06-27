using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NCodeRiddian.Input
{
    public class SimulatedController : GamePadStateManager
    {
        public SimulatedController()
        {
            Current = new SimulatedState();
            Previous = new SimulatedState();
        }

        public override void Update()
        {
            ControllerState tmp = Previous;
            Previous = Current;
            Current = Previous;
            Current.Copy(Previous);
        }
    }
    public class SimulatedState : ControllerState
    {
        public void SetButtonDown(ControllerButton b)
        {
            base.buttons |= b;
        }
        public void SetButtonUp(ControllerButton b)
        {
            base.buttons &= ~b;
        }
        public void SetLeftTrigger(float f)
        {
            base.leftTrigger = f;
        }
        public void SetRightTrigger(float f)
        {
            base.rightTrigger = f;
        }
        public void SetLeftStick(Vector2 v)
        {
            base.leftStick.V = v;
        }
        public void SetRightStick(Vector2 v)
        {
            base.rightStick.V = v;
        }
    }
    
}
