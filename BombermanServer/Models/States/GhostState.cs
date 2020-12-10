using System;
using System.Collections.Generic;

namespace BombermanServer.Models.States
{
    public abstract class GhostState
    {
        protected Ghost GhostContext { get; set; }
        protected int? LastTurnIndex { get; set; }
        protected Random randomGenerator { get; set; } = new Random();

        public abstract void Move(List<bool> allTurns = null);

        protected GhostState(Ghost context)
        {
            GhostContext = context;
        }
    }
}
