using System.Drawing;
using System.Text;

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

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var point in ExplosionCoords)
            {
                stringBuilder.Append($"X = {point.X} Y = {point.Y} | ");
            }
            return $"{OwnerId} {stringBuilder.ToString()}";
        }
    }
}