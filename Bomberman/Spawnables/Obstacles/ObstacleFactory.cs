using Bomberman.Spawnables.Obstacles.DestructableObstacles;
using Bomberman.Spawnables.Obstacles.UndestructableObstacles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables.Obstacles
{
    // ABSTRACT-FACTORY
    abstract class ObstacleFactory : GameApplication
    {
        public abstract DestroyableObstacle GetDestroyable(string destroyableObj);
        public abstract UndestroyableObstacle GetUndestroyable(string undestroyableObj);
    }
}
