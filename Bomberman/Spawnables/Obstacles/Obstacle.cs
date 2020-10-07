using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables.Obstacles
{
    class Obstacle
    {
        private string name { get; set; }
        private int health { get; set; }

        public int geHealth() { return this.health; }

        public void setHealth(int health) { this.health = health; }
    }
}
