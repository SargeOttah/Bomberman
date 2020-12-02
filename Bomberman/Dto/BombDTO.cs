using System.Drawing;
using SFML.Graphics;
using Bomberman.Spawnables.Weapons;

namespace Bomberman.Dto
{
    public class BombDTO
    {
        public string OwnerId { get; set; }
        public float Damage { get; set; }
        public float IgnitionDuration { get; set; }
        public int ExplosionRadius { get; set; }
        public PointF bombPosition { get; set; }

        public int CurrentBombType { get; set; }

        public BombDTO(string OwnerId, float Damage, float IgnitionDuration, int ExplosionRadius, PointF Position, int bombType)
        {
            this.OwnerId = OwnerId;
            this.Damage = Damage;
            this.IgnitionDuration = IgnitionDuration;
            this.ExplosionRadius = ExplosionRadius;
            this.bombPosition = Position;
            this.CurrentBombType = bombType;
        }

        public BombDTO()
        {

        }

        public override string ToString() => $"{OwnerId} {Damage} {IgnitionDuration} {ExplosionRadius} {bombPosition.X} {bombPosition.Y}";
    }
}
