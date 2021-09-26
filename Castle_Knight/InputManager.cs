using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Castle_Knight
{
    class InputManager
    {
        static KeyboardState currentKeyboardState;
        static KeyboardState prevoiusKeyboardState;
        static GamePadState[] currentGamePadState = new GamePadState[4];
        static GamePadState[] previousGamePadState = new GamePadState[4];
        static bool[] Connected = new bool[4];
        public enum Stick
        {
            Left, Right
        }
        /// <summary>
        ///Call this in every update method of your game to keep things up to date.
        /// </summary>
        public static void Update()
        {
            prevoiusKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            for (int i = 0; i < 4; i++)
            {
                previousGamePadState[i] = currentGamePadState[i];
                currentGamePadState[i] = GamePad.GetState((PlayerIndex)i);
                Connected[i] = GamePad.GetState((PlayerIndex)i).IsConnected;
            }
        }
        #region GamePad
        /// <summary>
        /// Checks if the button is being held down.
        /// </summary>
        /// <param name="button">The gamepad button you would like to check.</param>
        /// <returns>Returns a boolean value of rather the button is down or not.</returns>
        public static bool IsButtonDown(PlayerIndex controller, Buttons button)
        {
            return (currentGamePadState[(int)controller].IsButtonDown(button));
        }
        /// <summary>
        /// Checks if the button is up. (Not being held down)
        /// </summary>
        /// <param name="button">The gamepad button you would like to check.</param>
        /// <returns>Returns a boolean value of rather the button is up or not.</returns>
        public static bool IsButtonUp(PlayerIndex controller, Buttons button)
        {
            return (currentGamePadState[(int)controller].IsButtonUp(button));
        }
        /// <summary>
        /// Checks if the button has been pressed and released.
        /// </summary>
        /// <param name="button">The gamepad button you would like to check.</param>
        /// <returns>Returns a boolean value of rather the button has been pressed or not.</returns>
        public static bool IsButtonPressed(PlayerIndex controller, Buttons button)
        {
            return (currentGamePadState[(int)controller].IsButtonUp(button) &&
                previousGamePadState[(int)controller].IsButtonDown(button));
        }
        #endregion
        #region Keyboard
        /// <summary>
        /// Checks if a key is down.
        /// </summary>
        /// <param name="key">The key you would like to check.</param>
        /// <returns>Returns a boolean value of rather the key is down or not.</returns>
        public static bool IsKeyDown(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Checks if a key is up.
        /// </summary>
        /// <param name="key">The key you would like to check.</param>
        /// <returns>Returns a boolean value of rather the key is up or not.</returns>
        public static bool IsKeyUp(Keys key)
        {
            return (currentKeyboardState.IsKeyUp(key));
        }
        /// <summary>
        /// Checks if a key has been pressed and released.
        /// </summary>
        /// <param name="key">The key you would like to check.</param>
        /// <returns>Returns a boolean value of rather the key has been pressed and released.</returns>
        public static bool IsKeyPressed(Keys key)
        {
            return (currentKeyboardState.IsKeyUp(key) && prevoiusKeyboardState.IsKeyDown(key));
        }
        #endregion
    }
}
