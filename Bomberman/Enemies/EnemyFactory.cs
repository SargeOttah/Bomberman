using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Enemies
{
    class EnemyFactory
    {
        public Enemy createEnemy(string type)
        {
            Enemy enemy = null;
            if (type.Equals("Zombie")) {
                enemy = new Zombie();
            } 
            else if (type.Equals("Ghost")) {
                enemy = new Ghost();
            } 
            else if (type.Equals("Skeleton")) {
                enemy = new Skeleton();
            }

            return enemy;
        }
    }
}
  