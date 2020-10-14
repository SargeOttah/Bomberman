using Bomberman.Spawnables.Obstacles.DestructableObstacles;
using Bomberman.Spawnables.Obstacles.UndestructableObstacles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables.Obstacles
{
    // CONCRETE-FACTORY
    class DestroyableObstacleFactory : ObstacleFactory
    {
        public override UndestroyableObstacle GetUndestroyable(string undestroyableObj)
        {
            return null;
        }
        public override DestroyableObstacle GetDestroyable(string destroyableObj)
        {
            if (destroyableObj.Equals("Crate"))
            {
                return new Crate();
            }
            else if (destroyableObj.Equals("BrickWall"))
            {
                return new BrickWall();
            }
            return null;
        }
    }
}
