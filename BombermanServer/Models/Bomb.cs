using System.Drawing;

namespace BombermanServer.Models
{
    public class Bomb
    {
        public string OwnerId { get; set; } // connection id of owner
        public float Damage { get; set; }
        public float IgnitionDuration { get; set; }
        public int ExplosionRadius { get; set; }
        public PointF Position { get; set; }

        public Bomb()
        {

        }

        public override string ToString() => $"{OwnerId} {Damage} {IgnitionDuration} {ExplosionRadius} {Position.X} {Position.Y}";
    }
}
