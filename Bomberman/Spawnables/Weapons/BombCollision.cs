using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables.Weapons
{
    class BombCollision : BombImplementation
    {
        public void Deploy()
        {
            Console.WriteLine("Collision");
        }

        public string Explode()
        {
            return "ConcreteImplementationA: The result in platform A.\n";
        }
    }
}
