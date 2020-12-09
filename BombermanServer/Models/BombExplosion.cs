using System.Drawing;

namespace BombermanServer.Models
{
    public class BombExplosion
    {
        public string OwnerId { get; set; }
        public Point[] ExplosionCoords { get; private set; }

        public BombExplosion()
        {
            ExplosionCoords = new Point[4];
        }
    }
}