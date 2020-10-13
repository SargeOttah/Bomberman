using Bomberman.Collisions;
using Bomberman.Dto;
using SFML.Graphics;
using SFML.System;
using System.Drawing;
using System.Threading.Tasks;

//using Bomberman.Objects;
//using Bomberman.Utilities;

namespace Bomberman
{
    class Player : Sprite
    {
        public float Health { get; private set; } = 100;
        public Vector2f Speed { get; set; } = new Vector2f(0.0f, 0.0f);
        public bool IsDead { get; private set; } = false;
        public float SpeedMultiplier { get; private set; } = 1;

        public string connectionId { get; private set; }

        public Player() { }

        public Player(PlayerDTO playerDTO)
        {
            // TODO: scale player position based on current resolution
            this.Position = new Vector2f(playerDTO.position.X, playerDTO.position.Y); //new Vector2f(_renderWindow.Size.X / 2, _renderWindow.Size.Y / 2)
            this.TextureRect = new IntRect(0, 0, 19, 32);
            this.Scale = new Vector2f(3, 3);
            this.Texture = playerDTO.GetTexture();
            this.connectionId = playerDTO.connectionId;
        }

        public void UpdateStats(PlayerDTO playerDTO)
        {
            this.Position = new Vector2f(playerDTO.position.X, playerDTO.position.Y);
        }

        public void Translate(float xOffset, float yOffset)
        {
            this.Position = new Vector2f(this.Position.X + xOffset * SpeedMultiplier, this.Position.Y + yOffset * SpeedMultiplier);
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

        public void IncreaseMovementSpeed(float multiplier, float durationInMilis)
        {
            SpeedMultiplier = 2;
            Task.Delay((int)durationInMilis).ContinueWith(o => SpeedMultiplier = 1);
        }

        public PointF GetPointPosition()
        {
            return new PointF(this.Position.X, this.Position.Y);
        }

    }
}
