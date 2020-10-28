using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFML.Graphics;
using Bomberman.Global;
using System.Drawing;

namespace Bomberman.Dto
{
    class BombDTO : Sprite
    {
        public float Damage { get; private set; }
        public float PlaceSpeed { get; private set; }
        public float BombTimer { get; set; }


        public PointF myPosition { get; set; }

        public Sprite ProjectileSprite { get; set; }
        public Sprite ExplosionSprite { get; private set; }
        public BombDTO(float dmg, float placeDelay, float bombTimer, Sprite projectileSprite, PointF pos)
        {
            this.Damage = dmg;
            this.PlaceSpeed = placeDelay;
            this.BombTimer = bombTimer;
            this.ProjectileSprite = projectileSprite;
            this.myPosition = pos;
        }
    }
}
