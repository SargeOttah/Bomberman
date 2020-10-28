using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables.Weapons
{
    abstract class Weapon
    {

        protected BombImplementation _implementation;

        public Weapon(BombImplementation implementation)
        {
            this._implementation = implementation; // implements one of the actions
        }


        abstract public void Operation();
        //public virtual void Operation()
        //{
        //    _implementation.Deploy();
        //}
    }
}
