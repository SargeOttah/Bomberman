using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Bomberman.Spawnables.Obstacles.UndestructableObstacles
{
    class Stone : UndestroyableObstacle
    {
        private Sprite obstacle;

        public Stone() : base(17)
        {
            obstacle = CreateSprite();
        }

        public Sprite CreateSprite()
        {
            var tmpSprite = new Sprite();
            var tmpTexture = new Texture(Properties.Resources.stone);
            tmpSprite = new Sprite(tmpTexture);

            return tmpSprite;
        }

        public override Sprite SpawnObstacle()
        {
            return obstacle;
        }
    }
}
