using System.Drawing;

namespace BombermanServer.Models
{
    public class PlayerDTO
    {
        public int Id { get; set; }
        public string ConnectionId { get; set; }
        public PointF Position { get; set; }
        public int SpeedMultiplier { get; set; }
        public PlayerSprite Sprite { get; set; }
        public bool IsDead { get; set; }

        public PlayerDTO()
        {
        }
        
        public PlayerDTO(string connectionId) : this()
        {
            ConnectionId = connectionId;
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
