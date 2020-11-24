using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace BombermanServer.Models
{
    public class Bomb
    {
        public float Damage { get; set; }
        public float IgnitionDuration { get; set; }
        public int ExplosionRadius { get; set; }
        public PointF Position { get; set; }

        public override string ToString() => $"{Damage} {IgnitionDuration} {ExplosionRadius} {Position.X} {Position.Y}";
    }
}
