using Microsoft.Xna.Framework.Input;

namespace NCodeRiddian
{
    public abstract class KeyboardInputManager
    {
        private static KeyboardState oldState = Keyboard.GetState();
        private static KeyboardState newState = Keyboard.GetState();

        /// <summary>
        /// To be called at the start of every update the KeyboardManager will be used.
        /// </summary>
        public static void update()
        {
            oldState = newState;
            newState = Keyboard.GetState();
        }

        /// <summary>
        /// Returns if a key is currently pressed
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static bool isKeyDown(Keys k)
        {
            return newState.IsKeyDown(k);
        }

        /// <summary>
        /// Returns if a key was pressed this update
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static bool isKeyPressed(Keys k)
        {
            return newState.IsKeyDown(k) && oldState.IsKeyUp(k);
        }

        /// <summary>
        /// Returns if a key was released this update
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static bool isKeyReleased(Keys k)
        {
            return newState.IsKeyUp(k) && oldState.IsKeyDown(k);
        }
    }
}