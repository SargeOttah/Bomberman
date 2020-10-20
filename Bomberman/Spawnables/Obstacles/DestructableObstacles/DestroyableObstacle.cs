using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables.Obstacles.DestructableObstacles
{
    abstract class DestroyableObstacle : Obstacle
    {
        public abstract Sprite SpawnObstacle();
    }
}
