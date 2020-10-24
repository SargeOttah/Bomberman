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
        
        }
        public abstract Sprite SpawnObstacle();
    }
}
