using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Bomberman.Enemies
{
    class Ghost : Enemy
    {
        public Ghost ()
        {
            setName("Ghost");
            setDamage(1);
            setSprite(loadSprite());
        }

        private Sprite loadSprite()
        {
            var tmpSprite = new Sprite();
            var tmpTexture = new Texture(Properties.Resources.Ghost);
            tmpSprite = new Sprite(tmpTexture);

            return tmpSprite;
        }
    }
}
