using Bomberman.Properties;
using Bomberman.Global;
using SFML.Graphics;
using SFML.System;
using Bomberman.Dto;
using System.Drawing;
using System;

namespace Bomberman.Spawnables.Weapons
{
    public class Bomb : Sprite
    {
        public enum BombType : int // gimmick
        {
            RegularBomb = 0,
            SuperBomb = 1,
            FastBomb = 2
        }

        public float Damage { get; private set; }
        //public float PlaceSpeed { get; private set; }
        public float PlaceSpeed = 600f;
        public float IgnitionDuration { get; set; }

        public Clock DelayTimer { get; set; } = new Clock();         // Time until another bomb can be placed
        public Clock BombExplosionTimer { get; set; } = new Clock(); // Time until bomb explodes

        public Sprite ProjectileSprite { get; set; }
        public Sprite ExplosionSprite { get; private set; }

        public int CurrentBombType;

        public int ExplosionRadius = 5;

        // damage - placeDelay - bombTimer
        public Bomb(float dmg, float bombTimer, int currentBombType)
        {
            this.Damage = dmg;
            //this.PlaceSpeed = placeDelay;
            this.IgnitionDuration = bombTimer;
            CurrentBombType = currentBombType;

            // Loading default bomb texture
            this.ProjectileSprite = SpriteLoader.LoadSprite(Resources.bomb, new IntRect(0, 0, 64, 64));
            this.Origin = SpriteUtils.GetSpriteCenter(ProjectileSprite);

            // Loading explosion texture
            this.ExplosionSprite = SpriteLoader.LoadSprite(Resources.explosionSmall, new IntRect(0, 0, 64, 64));
            this.Texture = ExplosionSprite.Texture;
        }


        //(string OwnerId, float Damage, float IgnitionDuration, int ExplosionRadius, PointF Position, Sprite ProjectileSprite, Sprite ExplosionSp
        public BombDTO getBombDTO(string ownerID, PointF pos)
        {
            Console.WriteLine("Sending hub type: {0}", this.CurrentBombType);
            var bombDTO = new BombDTO(ownerID,
                                        this.Damage,
                                        this.IgnitionDuration,
                                        this.ExplosionRadius,
                                        pos,
                                        (int)this.CurrentBombType);

            return bombDTO;

        }
    }
}
