using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NCodeRiddian
{
    public abstract class Cursor
    {
        /// <summary>
        /// Indicates if the left mouse button is down
        /// </summary>
        public static bool leftDown = false;
        /// <summary>
        /// Indicates if the right mouse button is down
        /// </summary>
        public static bool rightDown = false;
        /// <summary>
        /// Indicates if the left mouse button was pressed this update
        /// </summary>
        public static bool leftPress = false;
        /// <summary>
        /// Indicates if the right mouse button was pressed this update
        /// </summary>
        public static bool rightPress = false;

        public static bool leftRelease
        {
            get
            {
                return currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed;
            }
        }

        public static bool rightRelease
        {
            get
            {
                return currentState.RightButton == ButtonState.Released && previousState.RightButton == ButtonState.Pressed;
            }
        }

        private static int lastWheel;
        private static int thisWheel;
        private static MouseState previousState = Mouse.GetState();
        private static MouseState currentState = Mouse.GetState();

        /// <summary>
        /// Called at the beginning of each update to update the cursor
        /// </summary>
        public static void update()
        {
            previousState = currentState;
            currentState = Mouse.GetState();

            lastWheel = thisWheel;
            thisWheel = Mouse.GetState().ScrollWheelValue;

            leftDown = currentState.LeftButton == ButtonState.Pressed;
            rightDown = currentState.RightButton == ButtonState.Pressed;

            leftPress = currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released;
            rightPress = currentState.RightButton == ButtonState.Pressed && previousState.RightButton == ButtonState.Released;
        }

        /// <summary>
        /// Returns the value of the mouse wheel
        /// </summary>
        /// <returns>The raw output of the mousewheel</returns>
        public static int getMouseWheel()
        {
            return Mouse.GetState().ScrollWheelValue;
        }

        /// <summary>
        /// Returns the amount by which the mouse wheel has changed this update
        /// </summary>
        /// <returns>The difference in mouse wheel value this update</returns>
        public static int getCurrentMouseWheel()
        {
            return thisWheel - lastWheel;
        }

        /// <summary>
        /// Returns the point on the screen where the mouse cursor currently is
        /// </summary>
        /// <returns>Screen location of the mouse</returns>
        public static Point screenLoc()
        {
            return new Point(Mouse.GetState().X, Mouse.GetState().Y);
        }

        /// <summary>
        /// Returns the point in the world (relative to the camera) where the mouse cursor currently is
        /// </summary>
        /// <returns>World location of the mouse</returns>
        public static Point worldLoc()
        {
            return new Point(Camera.getlocation().X + ((int)Math.Round(((float)Mouse.GetState().X - (float)Camera.getOrigin().X) / Camera.getZoom())), Camera.getlocation().Y + ((int)Math.Round(((float)Mouse.GetState().Y - (float)Camera.getOrigin().Y) / Camera.getZoom())));
        }
    }
}