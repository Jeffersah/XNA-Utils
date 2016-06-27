using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NCodeRiddian
{
    public abstract class GamePadStateManager
    {
        protected ControllerState Current;
        protected ControllerState Previous;
        public abstract void Update();

        public ControllerState BaseCurrent()
        {
            return Current;
        }
        public ControllerState BasePrevious()
        {
            return Previous;
        }

        public bool IsButtonDown(ControllerButton button)
        {
            return Current.IsButtonDown(button);
        }
        public bool IsButtonUp(ControllerButton button)
        {
            return Current.IsButtonUp(button);
        }
        public bool IsButtonPressed(ControllerButton button)
        {
            return Current.IsButtonDown(button) && Previous.IsButtonUp(button);
        }
        public bool IsButtonReleased(ControllerButton button)
        {
            return Current.IsButtonUp(button) && Previous.IsButtonDown(button);
        }
        public float LeftTrigger()
        {
            return Current.LeftTrigger;
        }
        public float RightTrigger()
        {
            return Current.RightTrigger;
        }
        public float DeltaLeftTrigger()
        {
            return Current.LeftTrigger - Previous.LeftTrigger;
        }
        public float DeltaRightTrigger()
        {
            return Current.RightTrigger - Previous.RightTrigger;
        }
        public Vector2 LeftStick()
        {
            return Current.LeftStick;
        }
        public Vector2 RightStick()
        {
            return Current.RightStick;
        }
        public Vector2 DeltaLeftStick()
        {
            return Current.LeftStick - Previous.LeftStick;
        }
        public Vector2 DeltaRightStick()
        {
            return Current.RightStick - Previous.RightStick;
        }
    }

    public class ControllerState
    {
        static Pair<Buttons, ControllerButton>[] CONVERT = {    new Pair<Buttons, ControllerButton>(Buttons.A, ControllerButton.A),
                                                                new Pair<Buttons, ControllerButton>(Buttons.B, ControllerButton.B),
                                                                new Pair<Buttons, ControllerButton>(Buttons.X, ControllerButton.X),
                                                                new Pair<Buttons, ControllerButton>(Buttons.Y, ControllerButton.Y),
                                                               new Pair<Buttons, ControllerButton>(Buttons.DPadUp, ControllerButton.Up),
                                                                new Pair<Buttons, ControllerButton>(Buttons.DPadDown, ControllerButton.Down),
                                                                new Pair<Buttons, ControllerButton>(Buttons.DPadLeft, ControllerButton.Left),
                                                                new Pair<Buttons, ControllerButton>(Buttons.DPadRight, ControllerButton.Right),
                                                               new Pair<Buttons, ControllerButton>(Buttons.Back, ControllerButton.Select),
                                                                new Pair<Buttons, ControllerButton>(Buttons.Start, ControllerButton.Start),
                                                                new Pair<Buttons, ControllerButton>(Buttons.BigButton, ControllerButton.Big),
                                                               new Pair<Buttons, ControllerButton>(Buttons.LeftStick, ControllerButton.LeftStick),
                                                                new Pair<Buttons, ControllerButton>(Buttons.LeftShoulder, ControllerButton.LeftShoulder),
                                                                new Pair<Buttons, ControllerButton>(Buttons.RightShoulder, ControllerButton.RightShoulder),
                                                                new Pair<Buttons, ControllerButton>(Buttons.RightStick, ControllerButton.RightStick)};

        protected ControllerButton buttons;
        protected float leftTrigger, rightTrigger;
        protected ControllerThumbstick leftStick;
        protected ControllerThumbstick rightStick;

        public ControllerState()
        {
            buttons = 0;
            leftTrigger = 0;
            rightTrigger = 0;
            leftStick = new ControllerThumbstick();
            rightStick = new ControllerThumbstick();
        }

        // Getters

        public float LeftTrigger { get { return leftTrigger; } }
        public float RightTrigger { get { return rightTrigger; } }
        public Vector2 LeftStick { get { return leftStick.V; } }
        public Vector2 RightStick { get { return rightStick.V; } }
        public float LeftX { get { return leftStick.X; } }
        public float RightX { get { return rightStick.X; } }
        public float LeftY { get { return leftStick.Y; } }
        public float RightY { get { return rightStick.Y; } }

        public bool IsButtonDown(ControllerButton Button)
        {
            return (buttons & Button) == Button;
        }
        public bool IsButtonUp(ControllerButton Button)
        {
            return !IsButtonDown(Button);
        }

        protected void ReadFromXNAState(GamePadState XNAState)
        {
            buttons = 0;
            foreach (Pair<Buttons, ControllerButton> pair in CONVERT)
            {
                if (XNAState.IsButtonDown(pair.first))
                {
                    buttons |= pair.second;
                }
            }
            leftTrigger = XNAState.Triggers.Left;
            rightTrigger = XNAState.Triggers.Right;
            leftStick.V = XNAState.ThumbSticks.Left;
            rightStick.V = XNAState.ThumbSticks.Right;
        }
        public void Copy(ControllerState c)
        {
            this.buttons = c.buttons;
            leftTrigger = c.leftTrigger;
            rightTrigger = c.rightTrigger;
            leftStick.V = c.leftStick.V;
            rightStick.V = c.rightStick.V;
        }
    }

    [Flags]
    public enum ControllerButton : long
    {
        // Button Pad
        A = 0x1,
        B = 0x10,
        X = 0x100,
        Y = 0x1000,
        // Center
        Select =    0x10000,
        Start =     0x100000,
        Big = 0x1000000,
        //D-Pad
        Left = 0x10000000,
        Right = 0x100000000,
        Up = 0x1000000000,
        Down = 0x10000000000,
        // Shoulders
        LeftShoulder = 0x100000000000,
        RightShoulder = 0x1000000000000,
        // Stick presses
        LeftStick = 0x10000000000000,
        RightStick = 0x100000000000000
    }

    public class ControllerThumbstick
    {
        public float X;
        public float Y;

        public Vector2 V { get { return new Vector2(X, Y); } set { X = value.X; Y = value.Y; } }
    }
}
