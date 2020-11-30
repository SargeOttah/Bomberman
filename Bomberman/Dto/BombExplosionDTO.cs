using System.Drawing;

namespace Bomberman.Dto
{
    public class BombExplosionDTO
    {
        public string OwnerId { get; set; }
        public Point[] ExplosionCoords { get; set; }
        //0 left
        //1 down
        //2 right
        //3 up

        public BombExplosionDTO()
        {
            ExplosionCoords = new Point[4];
        }
    }
}