using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables.Obstacles
{
    class FactoryPicker
    {
        public static ObstacleFactory GetFactory(string choice)
        {
            if (choice.Equals("Destroyable"))
            {
                return new DestroyableObstacleFactory();
            }
            else if (choice.Equals("Undestroyable"))
            {
                return new UndestroyableObstacleFactory();
            }
            return null;
        }
    }
}
