using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NCodeRiddian
{

    static class GamePadManager
    {
        static PlayerState[] playerStates = new PlayerState[4];
        static bool setupAlready = false;

        public static void update()
        {
            if (!setupAlready)
            {
                for (int x = 0; x < 4; x++)
                {
                    playerStates[x] = new PlayerState(x);
                }
                setupAlready = true;
            }
            for (int x = 0; x < 4; x++)
            {
                playerStates[x].update();
            }
        }

        private struct PlayerState
        {
            public PlayerIndex idx;
            public int intidx;
            public GamePadState previousState;
            public GamePadState currentState;

            public PlayerState(int x)
            {
                intidx = x;
                switch (x)
                {
                    case 1:
                        idx = PlayerIndex.One;
                        break;
                    case 2:
                        idx = PlayerIndex.Two;
                        break;
                    case 3:
                        idx = PlayerIndex.Three;
                        break;
                    case 4:
                        idx = PlayerIndex.Four;
                        break;
                    default:
                        idx = PlayerIndex.One;
                        break;
                }
                currentState = GamePad.GetState(idx);
                previousState = GamePad.GetState(idx);
            }

            public void update()
            {
                previousState = currentState;
                currentState = GamePad.GetState(idx);
            }
        }
    }
}
