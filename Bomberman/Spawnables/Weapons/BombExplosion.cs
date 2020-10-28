using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables.Weapons
{
    class BombExplosion : BombImplementation
    {
        public void Deploy()
        {
            Console.WriteLine("Boom");
        }

        public string Explode()
        {
            return "ConcreteImplementationA: The result in platform A.\n";
        }
    }
}
