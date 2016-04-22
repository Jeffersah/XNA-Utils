using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PixelCrusher
{
    public class ControllerManager
    {
        int[] b_down_time;
        int[] b_up_time;

        PlayerIndex index;
        GamePadState oldState;
        GamePadState newState;

        public ControllerManager(PlayerIndex index)
        {
            this.index = index;
            oldState = newState = GamePad.GetState(index);
            b_down_time = new int[Enum.GetNames(typeof(Buttons)).Length];
            b_up_time = new int[Enum.GetNames(typeof(Buttons)).Length];
            for(int i = 0; i < b_down_time.Length; i++)
            {
                b_down_time[i] = b_up_time[i] = 0;
            }
        }


        /// <summary>
        /// Must be called every update. Updates the ControllerManager
        /// </summary>
        public void Update()
        {
            oldState = newState;
            newState = GamePad.GetState(index);
            for (int i = 0; i < b_down_time.Length; i++)
            {
                if(ButtonDown((Buttons)i))
                {
                    b_down_time[i]++;
                    b_up_time[i] = 0;
                }
                else
                {
                    b_up_time[i]++;
                    b_down_time[i] = 0;
                }
            }
        }

        /// <summary>
        /// Checks if a button is currently down
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool ButtonDown(Buttons button)
        {
            return newState.IsButtonDown(button);
        }

        /// <summary>
        /// Checks if a button is currently up
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool ButtonUp(Buttons button)
        {
            return newState.IsButtonUp(button);
        }

        /// <summary>
        /// Checks if a button has been pressed this frame
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool ButtonPressed(Buttons button)
        {
            return newState.IsButtonDown(button) && oldState.IsButtonUp(button);
        }

        /// <summary>
        /// Checks if a button has been released this frame
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool ButtonReleased(Buttons button)
        {
            return newState.IsButtonUp(button) && oldState.IsButtonDown(button);
        }

        /// <summary>
        /// Returns the number of frames a button has been down for
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public int ButtonDownTime(Buttons button)
        {
            return b_down_time[(int)button];
        }

        /// <summary>
        /// Returns the number of frames a button has been up for
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public int ButtonUpTime(Buttons button)
        {
            return b_up_time[(int)button];
        }

        /// <summary>
        /// Get the XY coordinates of the left stick
        /// </summary>
        public Vector2 LeftStick
        {
            get
            {
                return newState.ThumbSticks.Left;
            }
        }
        
        /// <summary>
        /// Get the XY coordinates of the right stick
        /// </summary>
        public Vector2 RightStick
        {
            get
            {
                return newState.ThumbSticks.Right;
            }
        }

        /// <summary>
        /// Gets a vector representing the distance the left stick has moved this update
        /// </summary>
        public Vector2 DeltaLeftStick
        {
            get
            {
                return Vector2.Subtract(newState.ThumbSticks.Left, oldState.ThumbSticks.Left);
            }
        }


        /// <summary>
        /// Gets a vector representing the distance the right stick has moved this update
        /// </summary>
        public Vector2 DeltaRightStick
        {
            get
            {
                return Vector2.Subtract(newState.ThumbSticks.Right, oldState.ThumbSticks.Right);
            }
        }

        /// <summary>
        /// Get the current value of the left trigger
        /// </summary>
        public float LeftTrigger
        {
            get
            {
                return newState.Triggers.Left;
            }
        }

        /// <summary>
        /// Get the current value of the right trigger
        /// </summary>
        public float RightTrigger
        {
            get
            {
                return newState.Triggers.Right;
            }
        }

        /// <summary>
        /// Gets the change in the left trigger since the last frame
        /// </summary>
        public float DeltaLeftTrigger
        {
            get
            {
                return newState.Triggers.Left - oldState.Triggers.Left;
            }
        }

        /// <summary>
        /// Gets the change in the right trigger since the last frame
        /// </summary>
        public float DeltaRightTrigger
        {
            get
            {
                return newState.Triggers.Right - oldState.Triggers.Right;
            }
        }
    }
}
