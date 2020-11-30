
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables
{
    public class Spawnable : Drawable
    {
        public float TimeSinceCreation { get; set; }
        public float DespawnDrawableAfter { get; set; } = 1.5f;
        public Sprite ProjectileSprite { get; private set; }

        public Spawnable(Sprite projectileSprite, Vector2f position, float rotation)
        {
            this.ProjectileSprite = new Sprite(projectileSprite);
            TimeSinceCreation = 0;


            float xSize = projectileSprite.Scale.X * projectileSprite.TextureRect.Width;
            float ySize = projectileSprite.Scale.Y * projectileSprite.TextureRect.Height;
            var size = new Vector2f(xSize, ySize);
            ProjectileSprite.Origin = new Vector2f(size.X / 2.0f, size.Y / 2.0f);


            ProjectileSprite.Position = position;
            ProjectileSprite.Rotation = rotation;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTime">how much time has passed between frames in seconds</param>
        public void AddDeltaTime(float deltaTime)
        {
            TimeSinceCreation += deltaTime;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(ProjectileSprite);
        }
    }
}
