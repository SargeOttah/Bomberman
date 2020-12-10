using BombermanServer.Constants;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BombermanServer.Models.States.ConcreteStates
{
    public class ActiveGhostState : GhostState
    {
        public ActiveGhostState(Ghost context) : base(context) { }

        public override void Move(List<bool> allTurns = null)
        {
            if (allTurns is null) return;
            var availableTurnIndexes = new List<int>(); // Indexes of LEGAL moves (if we can only go up or right, this list will contain numbers 1 and 2).
            for (var i = 0; i < allTurns.Count; i++)
            {
                if (allTurns[i]) availableTurnIndexes.Add(i);
            }

            if (LastTurnIndex is null || !allTurns[LastTurnIndex.Value]) // Critical need to recalculate new turn - either it's the first move of the ghost or our previous direction isn't legal anymore.
            {
                LastTurnIndex = availableTurnIndexes[randomGenerator.Next(availableTurnIndexes.Count)];
            }
            else if (availableTurnIndexes.Count > 2) // If we recalculate only on the previous condition, we recalculate only in the corners of the border and ghost never enters the middle - this condition fixes that.
            {
                var newTurnIndex = availableTurnIndexes[randomGenerator.Next(availableTurnIndexes.Count)];
                if (Math.Abs(LastTurnIndex.Value - newTurnIndex) != 2) // Unlike in previous condition, here we have the possibility to go back even though no obstacle was met - this could create wiggling and we never want that (this is why indexes of directions matter in allTurns!)
                {
                    LastTurnIndex = newTurnIndex;
                }
            }

            GhostContext.Position = LastTurnIndex switch // Choose the direction based on (possibly) recalculated index (this is also why indexes matter).
            {
                0 => new PointF(GhostContext.Position.X - MapConstants.tileSize, GhostContext.Position.Y),
                1 => new PointF(GhostContext.Position.X, GhostContext.Position.Y - MapConstants.tileSize),
                2 => new PointF(GhostContext.Position.X + MapConstants.tileSize, GhostContext.Position.Y),
                3 => new PointF(GhostContext.Position.X, GhostContext.Position.Y + MapConstants.tileSize),
                _ => GhostContext.Position
            };
        }
    }
}
