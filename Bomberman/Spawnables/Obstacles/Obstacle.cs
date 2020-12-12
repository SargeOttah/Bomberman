using Bomberman.Map;
using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Spawnables.Tiles;

namespace Bomberman.Spawnables.Obstacles
{
    public class Obstacle : Ground
    {
        public Obstacle(int textureIdx) : base(textureIdx)
        {

        }
        
        private bool isDestroyable { get; set; }
        
        public bool getIsDestroyable() { return this.isDestroyable; }
    }
}
