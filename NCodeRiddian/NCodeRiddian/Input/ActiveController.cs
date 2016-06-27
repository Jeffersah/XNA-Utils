using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace NCodeRiddian.Input
{
    public class ActiveController : GamePadStateManager
    {
        public PlayerIndex PlayerIdx;
        public ActiveController(PlayerIndex PlayerIdx)
        {
            this.PlayerIdx = PlayerIdx;
            Previous = new ActiveState();
            Current = new ActiveState();
        }

        public override void Update()
        {
            // Swap states, save memory.
            ControllerState tmp = Previous;
            Previous = Current;
            Current = tmp;
            ((ActiveState)Current).ReadFromXNA(GamePad.GetState(PlayerIdx));
        }
    }

    public class ActiveState : ControllerState
    {
        public void ReadFromXNA(GamePadState state)
        {
            base.ReadFromXNAState(state);
        }
    }
}
