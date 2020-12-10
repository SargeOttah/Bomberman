using System.Collections.Generic;
using System.Drawing;

namespace BombermanServer.Models.States.ConcreteStates
{
    public class DeadGhostState : GhostState
    {
        public DeadGhostState(Ghost context) : base(context) { }

        public override void Move(List<bool> allTurns = null)
        {
            GhostContext.Position = new PointF(float.NaN, float.NaN);
        }
    }
}
