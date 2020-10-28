using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFML.Graphics;
using BombermanServer.Utils;
using System.Drawing;

namespace BombermanServer.Models
{
    public class BombDTO : Sprite
    {
        public float Damage { get; private set; }
        public float PlaceSpeed { get; private set; }
        public float BombTimer { get; set; }


        public PointF myPosition { get; set; }

        public Sprite ProjectileSprite { get; set; }
        public Sprite ExplosionSprite { get; private set; }

        // damage - placeDelay - bombTimer
        //public Bomb(float dmg, float placeDelay, float bombTimer)
        //{
        //    this.Damage = dmg;
        //    this.PlaceSpeed = placeDelay;
        //    this.BombTimer = bombTimer;

        //    // Loading bomb texture
        //    this.ProjectileSprite = SpriteLoader.LoadSprite(Resources.bomb, new IntRect(0, 0, 64, 64));
        //    this.Origin = SpriteUtils.GetSpriteCenter(ProjectileSprite);

        //    // Loading explosion texture
        //    this.ExplosionSprite = SpriteLoader.LoadSprite(Resources.explosionSmall, new IntRect(0, 0, 64, 64));
        //    this.Texture = ExplosionSprite.Texture;
        //}
    }
}
