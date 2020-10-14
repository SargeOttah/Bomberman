using Bomberman.Collisions;
using Bomberman.Dto;
using Bomberman.Spawnables;
using SFML.Graphics;
using SFML.System;
using System.Drawing;
using System.Threading.Tasks;

namespace Bomberman
{
    class Player : Sprite
    {
        public float Health { get; private set; } = 100;
        public Vector2f Speed { get; set; } = new Vector2f(0.0f, 0.0f);
        public bool IsDead { get; private set; } = false;
        public float SpeedMultiplier { get; private set; } = 1;
        
        public string connectionId { get; private set; }
        public Bomb Bomb { get; set; }

        //Debug
        private RectangleShape debugShape { get; set; }

        private bool boolDebug = false;
        public Player() { }

        public Player(PlayerDTO playerDTO)
        {
            // TODO: scale player position based on current resolution
            this.Position = new Vector2f(playerDTO.position.X, playerDTO.position.Y); //new Vector2f(_renderWindow.Size.X / 2, _renderWindow.Size.Y / 2)
            this.TextureRect = new IntRect(0, 0, 19, 32);
            this.Scale = new Vector2f(3, 3);
            this.Texture = playerDTO.GetTexture();
            this.connectionId = playerDTO.connectionId;
            this.Origin = GetSpriteCenter(this);


            InitDebug();
        }

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
            
            if(boolDebug)
            {
                var nonOriginPos = new Vector2f(this.GetGlobalBounds().Left, this.GetGlobalBounds().Top);
                debugShape.Position = nonOriginPos;
            }
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

        public bool CheckMovementCollision(float xOffset, float yOffset, Sprite targetCollider)
        {
            

            Translate(xOffset, yOffset);
            if (CollisionTester.BoundingBoxTest(this, targetCollider))
            {
                Translate(-xOffset, -yOffset);
                return true;
            }
            else
            {
                Translate(-xOffset, -yOffset);
                return false;
            }
            
        }

        public bool CheckCollisions()
        {
            bool tmpDeath = false;
            foreach (Spawnable item in Bomb.BombTriggers)
            {
                if (this.GetGlobalBounds().Intersects(item.ProjectileSprite.GetGlobalBounds()))
                {
                    this.Position = new Vector2f(0f, 0f); // Presume death
                    tmpDeath = true;
                    
                }
            }
            return tmpDeath;
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

    }
}
