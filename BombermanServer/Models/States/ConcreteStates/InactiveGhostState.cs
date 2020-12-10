using BombermanServer.Constants;
using System.Collections.Generic;
using System.Drawing;

namespace BombermanServer.Models.States.ConcreteStates
{
    public class InactiveGhostState : GhostState
    {
        public InactiveGhostState(Ghost context) : base(context) { }

        public override void Move(List<bool> allTurns = null)
        {
            GhostContext.Position = new PointF(6.5f * MapConstants.tileSize, 5.5f * MapConstants.tileSize);

            LastTurnIndex = null;
        }
    }
}
