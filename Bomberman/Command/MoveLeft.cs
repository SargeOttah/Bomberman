﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Command
{
    // Concrete Simple command
    class MoveLeft : CommandTemplate
    {
        // Move the player
        public override void Move(Player player, float moveDistance)
        {
            player.Translate(moveDistance, 0);
        }
    }
}
