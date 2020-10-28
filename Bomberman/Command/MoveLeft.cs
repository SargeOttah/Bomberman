using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Command
{
    // Concrete Simple command
    class MoveLeft : IMovement
    {
        //Called when we press a key
        public void Execute(Player player, float moveDistance)
        {
            Move(player, moveDistance);
        }

        // Move the player
        public void Move(Player player, float moveDistance)
        {
            player.Translate(moveDistance, 0);
        }
    }
}
