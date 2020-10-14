using Bomberman.Spawnables.Obstacles.UndestructableObstacles;
using Bomberman.Spawnables.Obstacles.DestructableObstacles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables.Obstacles
{
    // CONCRETE-FACTORY
    class UndestroyableObstacleFactory : ObstacleFactory
    {
        public override DestroyableObstacle GetDestroyable(string destroyableObj)
        {
            return null;
        }
        public override UndestroyableObstacle GetUndestroyable(string undestroyableObj)
        {
            if (undestroyableObj.Equals("Obsidian"))
            {
                return new Obsidian();
            }
            else if (undestroyableObj.Equals("Stone"))
            {
                return new Stone();
            }
            return null;
        }
    }
}
