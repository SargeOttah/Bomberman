using BombermanServer.Constants;
using BombermanServer.Models.States;

namespace BombermanServer.Models
{
    public class Ghost
    {
        public static readonly float StartingX = 4.5f * MapConstants.tileSize;
        public static readonly float StartingY = 5.5f * MapConstants.tileSize;

        public float? X { get; set; }
        public float? Y { get; set; }
        public GhostState State { get; set; }

        public void Move()
        {
            State.Move();
        }

        public void UpdateState()
        {
            State.UpdateState();
        }
    }
}
