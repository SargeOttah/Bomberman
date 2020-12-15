using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Command
{
    abstract class CommandTemplate : IMovement
    {
        public void Execute(Player player, float moveDistance)
        {
            Move(player, moveDistance);
        }

        public abstract void Move(Player player, float moveDistance);
    }
}
