using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables.Obstacles.DestructableObstacles
{
    abstract class DestroyableObstacle : Obstacle
    {
        public DestroyableObstacle(int textureIdx) : base(textureIdx)
        {
            setHealth(1);
        }
        public abstract Sprite SpawnObstacle();
    }
}
