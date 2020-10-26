using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Global;
using Bomberman.Properties;
using SFML.Graphics;
using SFML.System;

namespace Bomberman.Spawnables.Weapons
{
    class RegularBomb : Bomb
    {
        public RegularBomb() : base(20, 500, 2000)
        {
            setSprite();
        }

        private void setSprite()
        {
            this.ProjectileSprite = SpriteLoader.LoadSprite(Resources.bomb, new IntRect(0, 0, 64, 64));
        }
    }
}
