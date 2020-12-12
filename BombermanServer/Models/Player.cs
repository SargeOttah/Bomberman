using System.Drawing;
using BombermanServer.Models.Snapshots;

namespace BombermanServer.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string ConnectionId { get; set; }
        public PointF Position { get; set; }
        public int SpeedMultiplier { get; set; }
        public PlayerSprite Sprite { get; set; }
        public bool IsDead { get; set; }

        private PlayerSnapshot _snapshot;

        private Player()
        {

        }
        
        public Player(string connectionId) : this()
        {
            ConnectionId = connectionId;
        }

        public void MakeSnapshot()
        {
            _snapshot = new PlayerSnapshot(Id, ConnectionId, Position, SpeedMultiplier, Sprite, this);
        }

        public override string ToString() => $"{Id} {ConnectionId} {Position.X} {Position.Y} {Sprite}";
    }

    public enum PlayerSprite
    {
        Blue = 0,
        Green = 1,
        Red = 2
    }
}
