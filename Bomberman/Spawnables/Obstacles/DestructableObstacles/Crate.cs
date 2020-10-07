using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables.Obstacles.DestructableObstacles
{
    class Crate : DestroyableObstacle
    {
        private Sprite obstacle;

        public Crate()
        {
            obstacle = CreateSprite();
        }

        public Sprite CreateSprite()
        {
            var tmpSprite = new Sprite();
            var tmpTexture = new Texture(Properties.Resources.crate);
            tmpSprite = new Sprite(tmpTexture);

            return tmpSprite;
        }

        public override Sprite SpawnObstacle()
        {
            return obstacle;
        }
    }
}
