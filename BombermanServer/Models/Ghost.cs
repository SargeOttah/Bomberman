using BombermanServer.Models.States;
using System.Collections.Generic;
using System.Drawing;

namespace BombermanServer.Models
{
    public class Ghost
    {
        public PointF Position { get; set; }
        public GhostState State { get; set; }

        public void Move(List<bool> allTurns = null)
        {
            State.Move(allTurns);
        }
    }
}
