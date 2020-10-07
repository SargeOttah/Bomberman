using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Bomberman.Enemies
{
    class Zombie : Enemy
    {
        public Zombie()
        {
            setName("Zombie");
            setDamage(1);
            setSprite(loadSprite());
        }

        private Sprite loadSprite()
        {
            var tmpSprite = new Sprite();
            var tmpTexture = new Texture(Properties.Resources.zombie_front);
            tmpSprite = new Sprite(tmpTexture);

            return tmpSprite;
        }
    }
}
