using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Bomberman.Spawnables.Obstacles.UndestructableObstacles
{
    class Obsidian : UndestroyableObstacle
    {
        private Sprite obstacle;

        public Obsidian()
        {
            obstacle = CreateSprite();
        }

        public Sprite CreateSprite()
        {
            var tmpSprite = new Sprite();
            var tmpTexture = new Texture(Properties.Resources.obsidian);
            tmpSprite = new Sprite(tmpTexture);

            return tmpSprite;
        }

        public override Sprite SpawnObstacle()
        {
            return obstacle;
        }
    }
}
