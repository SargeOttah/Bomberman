using System;

namespace BombermanServer.Models.States
{
    public abstract class GhostState
    {
        protected Ghost GhostContext { get; set; }
        protected int? LastTurnIndex { get; set; }
        protected Random RandomGenerator { get; set; } = new Random();

        public abstract void Move();
        public abstract void UpdateState();

        protected GhostState(Ghost context)
        {
            GhostContext = context;
        }
    }
}
