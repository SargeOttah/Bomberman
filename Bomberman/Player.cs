using Bomberman.Collisions;
using Bomberman.Dto;
using Bomberman.Map;
using Bomberman.Spawnables;
using Bomberman.Spawnables.Weapons;
using Bomberman.Spawnables.Obstacles;
using SFML.Graphics;
using SFML.System;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Bomberman
{
    public class Player : Sprite
    {
        //private readonly int[,] directions = new int[4, 2]{
        //    { -1, 0 }, // left
        //    { 0, 1 }, // down
        //    { 1, 0 }, // right
        //    { 0, -1 }, // up
        //};
        private readonly int[][] directions = new int[][]{
            new int[] { -1, 0  },
            new int[] {  0, 1  },
            new int[] {  1, 0  },
            new int[] {  0, -1 }
        };


        public float Health { get; private set; } = 100;
        public Vector2f Speed { get; set; } = new Vector2f(0.0f, 0.0f);
        
        public float SpeedMultiplier { get; private set; } = 1;

        public Vector2f playerSpawn;

        public string connectionId { get; private set; }
        public Bomb Bomb { get; set; }

        public int Level = 0;
        public bool IsDead { get; private set; } = false;

        //Debug
        private RectangleShape debugShape { get; set; }

        private bool boolDebug = false;

        // Drawable list
        public List<Spawnable> Spawnables = new List<Spawnable>();
        // Bomb collisions
        public List<Spawnable> BombTriggers = new List<Spawnable>();


        public Player() { }

        public Player(PlayerDTO playerDTO)
        {
            // TODO: scale player position based on current resolution
            this.Position = new Vector2f(playerDTO.position.X, playerDTO.position.Y); //new Vector2f(_renderWindow.Size.X / 2, _renderWindow.Size.Y / 2)
            this.TextureRect = new IntRect(0, 0, 19, 32);
            this.Scale = new Vector2f(3, 1.7f);
            this.Texture = playerDTO.GetTexture();
            this.connectionId = playerDTO.connectionId;
            this.Origin = GetSpriteCenter(this);

            this.playerSpawn = new Vector2f(playerDTO.position.X, playerDTO.position.Y);



            InitDebug();
        }


        // BOMBS
        // -----------------

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

                    // Changing to Bomb.IgnitionDuration instead p.DespawnDrawableAfter
                    // removes Bomb decals too fast or slow (aka. local timescale instead global)

                    if (p.TimeSinceCreation > (float)(p.DespawnDrawableAfter / 1000))
                    {
                        // SPAWN EXPLOSIONS - just before bomb erases
                        Bomb.BombExplosionTimer.Restart(); // restart wait clock when placed
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

        public void CreateExplosion(BombExplosionDTO bombExplosionDTO)
        {
            var explosionCoords = bombExplosionDTO.ExplosionCoords;
            Spawnable explosion = new Spawnable(Bomb.ExplosionSprite, this.Position, this.Rotation);
            explosion.ProjectileSprite.Position = CalculateMapPos(explosionCoords[1].X, explosionCoords[0].Y); // set flame position to bomb
            explosion.DespawnDrawableAfter = .5f; // despawn flame after
            BombTriggers.Add(explosion);
            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int x = explosionCoords[1].X + directions[i][0];
                int y = explosionCoords[0].Y + directions[i][1];
                while (x != explosionCoords[i].X || y != explosionCoords[i].Y)
                {
                    explosion = new Spawnable(Bomb.ExplosionSprite, this.Position, this.Rotation);
                    explosion.ProjectileSprite.Position = CalculateMapPos(x, y); // set flame position to bomb
                    explosion.DespawnDrawableAfter = .5f; // despawn flame after
                    BombTriggers.Add(explosion);
                    x += directions[i][0];
                    y += directions[i][1];
                }
            }
        }

        static private Vector2f CalculateMapPos(int x, int y)
        {
            return new Vector2f(x * MapConstants.tileSize + MapConstants.tileSize / 2,
                                y * MapConstants.tileSize + MapConstants.tileSize / 2);
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

        public void AddBomb(BombDTO bomb)
        {
            // global bomb - doesnt know if its player bomb or enemy
            var tmpBomb = new Bomb(20, 2500, 0);

            if (bomb.CurrentBombType == 2)
            {
                tmpBomb = new FastBomb();
            }
            else if (bomb.CurrentBombType == 1)
            {
                tmpBomb = new SuperBomb();
            }

            Spawnable spawnable = new Spawnable(tmpBomb.ProjectileSprite, new Vector2f(bomb.bombPosition.X, bomb.bombPosition.Y), this.Rotation);
            spawnable.DespawnDrawableAfter = tmpBomb.IgnitionDuration; // set type specific speed

            Spawnables.Add(spawnable);
        }


        // BOMBS
        // -----------------

        public static Vector2f GetSpriteCenter(Sprite sprite)
        {
            Vector2f size = GetSpriteSize(sprite);
            return new Vector2f(size.X / 2.0f, size.Y / 2.0f);
        }

        public static Vector2f GetSpriteSize(Sprite sprite)
        {
            float xSize = sprite.TextureRect.Width;
            float ySize = sprite.TextureRect.Height;


            return new Vector2f(xSize, ySize);
        }

        public void UpdateStats(PlayerDTO playerDTO)
        {
            this.Position = new Vector2f(playerDTO.position.X, playerDTO.position.Y);
        }

        public void Translate(float xOffset, float yOffset)
        {
            this.Position = new Vector2f(this.Position.X + xOffset * SpeedMultiplier, this.Position.Y + yOffset * SpeedMultiplier);

            if (boolDebug)
            {
                var nonOriginPos = new Vector2f(this.GetGlobalBounds().Left, this.GetGlobalBounds().Top);
                debugShape.Position = nonOriginPos;
            }
        }

        public BombDTO getBombDTO() // get bomb in data transfer format
        {
            return Bomb.getBombDTO(this.connectionId, GetPointPosition());
        }

        //DebugBox
        public RectangleShape DrawFrame()
        {
            boolDebug = true;
            return debugShape;
        }
        public void InitDebug()
        {
            Vector2f tempDim = new Vector2f(this.GetGlobalBounds().Width, this.GetGlobalBounds().Height);
            debugShape = new RectangleShape(tempDim);
            debugShape.FillColor = SFML.Graphics.Color.Transparent;
            debugShape.OutlineColor = SFML.Graphics.Color.Red;
            debugShape.OutlineThickness = 1f;
        }

        public bool CheckMovementCollision(float xOffset, float yOffset, List<Obstacle> obstacles)
        {
            foreach (Obstacle obstacle in obstacles)
            { // kinda works
                Translate(xOffset, yOffset);
                if (CollisionTester.TileBoundingBoxTest(this, obstacle))
                {
                    Translate(-xOffset, -yOffset);
                    return true;
                }
                else
                {
                    Translate(-xOffset, -yOffset);
                }
            }
            return false;
        }

        public bool CheckDeathCollisions()
        {
            //bool tmpDeath = false;
            
            foreach (Spawnable item in BombTriggers)
            {
                if (this.GetGlobalBounds().Intersects(item.ProjectileSprite.GetGlobalBounds()))
                {
                    
                    this.Position = playerSpawn; // Presume death
                    // death now handled outside player class as a Command
                    //tmpDeath = true;
                    this.IsDead = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public void IncreaseMovementSpeed(float multiplier, float durationInMilis)
        {
            SpeedMultiplier = 2;
            Task.Delay((int)durationInMilis).ContinueWith(o => SpeedMultiplier = 1);
        }

        public void Update() // update spawnables
        {
            UpdateBomb();
            //..//
        }

        public void UpdateBomb()
        {
            if (Bomb != null)
            {
                Bomb.Position = this.Position; // Bomb placement coord = Player coord.
                //Bomb.ExplosionSprite.Position = Bomb.Position;
            }
        }


        public PointF GetPointPosition()
        {
            return new PointF(this.Position.X, this.Position.Y);
        }
        public Vector2f GetVectorPosition()
        {
            return this.Position;
        }
    }
}
