using System.Drawing;

namespace Bomberman.Dto
{
    class BombDTO
    {
        public float Damage { get; set; }
        public float IgnitionDuration { get; set; }
        public int ExplosionRadius { get; set; }
        public PointF Position { get; set; }

        public BombDTO(float Damage, float IgnitionDuration, int ExplosionRadius, PointF Position)
        {
            this.Damage = Damage;
            this.IgnitionDuration = IgnitionDuration;
            this.ExplosionRadius = ExplosionRadius;
            this.Position = Position;
        }
    }
}
