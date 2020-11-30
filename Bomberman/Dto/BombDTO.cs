using System.Drawing;

namespace Bomberman.Dto
{
    public class BombDTO
    {
        public string OwnerId { get; set; }
        public float Damage { get; set; }
        public float IgnitionDuration { get; set; }
        public int ExplosionRadius { get; set; }
        public PointF Position { get; set; }

        public BombDTO(string OwnerId, float Damage, float IgnitionDuration, int ExplosionRadius, PointF Position)
        {
            this.OwnerId = OwnerId;
            this.Damage = Damage;
            this.IgnitionDuration = IgnitionDuration;
            this.ExplosionRadius = ExplosionRadius;
            this.Position = Position;
        }

        public BombDTO()
        {

        }


    }
}
