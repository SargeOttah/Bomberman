using Bomberman.Map;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bomberman.Spawnables.Obstacles
{
    public class Obstacle : Tile
    {
        public Obstacle(int textureIdx) : base(textureIdx)
        {

        }
        
        private bool isDestroyable { get; set; }
        
        public bool getIsDestroyable() { return this.isDestroyable; }
    }
}
