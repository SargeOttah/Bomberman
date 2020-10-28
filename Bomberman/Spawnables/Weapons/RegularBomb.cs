using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Global;
using Bomberman.Properties;
using SFML.Graphics;
using SFML.System;

namespace Bomberman.Spawnables.Weapons
{
    class RegularBomb : Weapon
    {
        public RegularBomb(BombImplementation implementation) : base(implementation)
        {

        }

        public override void Operation()
        {
            Console.WriteLine("Generating Regular bomb");


            _implementation.Deploy();
        }

    }
}
