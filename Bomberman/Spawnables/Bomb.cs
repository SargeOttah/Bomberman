using System;
using System.Collections.Generic;
using Bomberman.Properties;
using Bomberman.Global;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System.Text;
using System.Linq;
using System.Numerics;
using Bomberman;



namespace Bomberman.Spawnables
{
    class Bomb : Sprite
    {
        public float Damage { get; private set; }
        public float PlaceSpeed { get; private set; }
        public float BombTimer { get; set; }

        // Drawable list
        public List<Spawnable> Spawnables { get; set; }

        // Bomb collisions
        public List<Spawnable> BombTriggers { get; set; }


        public Clock DelayTimer { get; set; } = new Clock();
        public Clock BombExplosionTimer { get; set; } = new Clock();

        public Sprite ProjectileSprite { get; private set; }
        public Sprite ExplosionSprite { get; private set; }

        // damage - placeDelay - bombTimer
        public Bomb(float dmg, float placeDelay, float bombTimer)
        {
            this.Damage = dmg;
            this.PlaceSpeed = placeDelay;
            this.BombTimer = bombTimer;

            // Bomb sprite and blast collidables init
            this.Spawnables = new List<Spawnable>();    // bomb Sprites
            this.BombTriggers = new List<Spawnable>();  // bomb Triggers

            // Loading bomb texture
            this.ProjectileSprite = SpriteLoader.LoadSprite(Resources.bomb, new IntRect(0, 0, 64, 64));
            this.Origin = SpriteUtils.GetSpriteCenter(ProjectileSprite);

            // Loading explosion texture
            this.ExplosionSprite = SpriteLoader.LoadSprite(Resources.explosionSmall, new IntRect(0, 0, 64, 64));
            this.Texture = ExplosionSprite.Texture;
        }
        
        public void DrawSpawnables(RenderWindow gameWindow)
        {
            for (int i = 0; i < Spawnables.Count; i++)
            {
                if (Spawnables[i] != null)
                {
                    gameWindow.Draw(Spawnables[i]);
                }
            }
        }
        public void DrawExplosions(RenderWindow gameWindow)
        {
            for (int i = 0; i < BombTriggers.Count; i++)
            {
                if (BombTriggers[i] != null)
                {
                    gameWindow.Draw(BombTriggers[i]);
                }
            }
        }

        public void UpdateSpawnables(float deltaTimeInSeconds)
        {
            // EXPLODE
            for (int i = 0; i < Spawnables.Count; i++)
            {
                if (Spawnables[i] != null)
                {
                    Spawnable p = Spawnables[i];
                    if (p.TimeSinceCreation > p.DespawnDrawableAfter)
                    {
                        // SPAWN EXPLOSIONS - just before bomb erases
                        BombExplosionTimer.Restart(); // restart wait clock when placed
                        Spawnable explosion = new Spawnable(ExplosionSprite, this.Position, this.Rotation);
                        explosion.ProjectileSprite.Position = p.ProjectileSprite.Position; // set flame position to bomb
                        explosion.DespawnDrawableAfter = .5f; // despawn flame after
                        BombTriggers.Add(explosion);


                        Spawnables.RemoveAt(i); // pop expired bomb
                    }
                    else
                    {
                        p.AddDeltaTime(deltaTimeInSeconds);
                    }
                }
                
            }

            // Remove old explosions
            UpdateBombColliders(deltaTimeInSeconds); // remove collider
        }

        public void PlaceBomb(Vector2f target)
        {
            if (DelayTimer.ElapsedTime.AsMilliseconds() > PlaceSpeed)
            {
                DelayTimer.Restart(); // restart wait clock when placed
                Spawnable bomb = new Spawnable(ProjectileSprite, target, this.Rotation);
                //Spawnable bomb = new Spawnable(ProjectileSprite, this.Position, this.Rotation);
                Spawnables.Add(bomb);
            }
        }
        public void UpdateBombColliders(float deltaTimeInSeconds)
        {
            for (int i = 0; i < BombTriggers.Count; i++)
            {
                if (BombTriggers[i] != null)
                {
                    Spawnable p = BombTriggers[i];
                    if (p.TimeSinceCreation > p.DespawnDrawableAfter)
                    {
                        BombTriggers.RemoveAt(i); // pop expired collider
                    }
                    else
                    {
                        p.AddDeltaTime(deltaTimeInSeconds);
                    }
                }

            }
        }
    }
}
