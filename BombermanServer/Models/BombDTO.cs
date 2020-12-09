using System.Drawing;

namespace BombermanServer.Models
{
    public class BombDTO
    {
        public string OwnerId { get; set; }
        public float Damage { get; set; }
        public float IgnitionDuration { get; set; }
        public int ExplosionRadius { get; set; }
        public PointF Position { get; set; }
        public int CurrentBombType { get; set; }

        public BombDTO()
        {

        }


        public override string ToString() => $"{OwnerId} {Damage} {IgnitionDuration} {ExplosionRadius} {Position.X} {Position.Y} {CurrentBombType}";
    }
}
