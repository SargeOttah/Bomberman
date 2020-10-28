using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Global;
using Bomberman.Properties;
using SFML.Graphics;
using SFML.System;

namespace Bomberman.Spawnables.Weapons
{
    class SuperBommb : Weapon
    {
        public SuperBommb(BombImplementation implementation) : base(implementation)
        {

        }

        public override void Operation()
        {
            Console.WriteLine("Generating SuperBomb bomb");

            _implementation.Deploy();
        }

    }
}
