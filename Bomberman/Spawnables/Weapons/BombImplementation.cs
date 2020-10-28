using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables.Weapons
{
    public interface BombImplementation
    {
        //string OperationImplementation();

        abstract public void Deploy();
        string Explode();
    }
}
