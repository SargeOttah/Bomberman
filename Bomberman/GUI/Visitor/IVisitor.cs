using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Spawnables.Enemies;
using Bomberman.Spawnables.Weapons;

namespace Bomberman.GUI
{
    public interface IVisitor
    {
        public void visit(Player player);
        public void visit(Bomb bomb);
        public string getData();
    }

}
