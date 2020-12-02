using System.Drawing;
using SFML.Graphics;

namespace BombermanServer.Models
{
    public class BombDTO
    {
        public string OwnerId { get; set; }
        public float Damage { get; set; }
        public float IgnitionDuration { get; set; }
        public int ExplosionRadius { get; set; }
        public PointF bombPosition { get; set; }

        public int CurrentBombType { get; set; }


        public override string ToString() => $"{OwnerId} {Damage} {IgnitionDuration} {ExplosionRadius} {bombPosition.X} {bombPosition.Y} {CurrentBombType}";
    }
}
