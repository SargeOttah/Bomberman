using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Bomberman.Spawnables.Obstacles.DestructableObstacles
{
    class BrickWall : DestroyableObstacle
    {
        private Sprite obstacle;

        public BrickWall()
        {
            obstacle = CreateSprite();
        }

        public Sprite CreateSprite()
        {
            var tmpSprite = new Sprite();
            var tmpTexture = new Texture(Properties.Resources.DesolatedHut);
            tmpSprite = new Sprite(tmpTexture);

            return tmpSprite;
        }

        public override Sprite SpawnObstacle()
        {
            return obstacle;
        }
    }
}
