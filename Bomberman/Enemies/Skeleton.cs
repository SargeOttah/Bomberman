using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Bomberman.Enemies
{
    class Skeleton : Enemy
    {
        public Skeleton()
        {
            setName("Skeleton");
            setDamage(1);
            setSprite(loadSprite());
        }

        private Sprite loadSprite()
        {
            var tmpSprite = new Sprite();
            var tmpTexture = new Texture(Properties.Resources.skeleton_front);
            tmpSprite = new Sprite(tmpTexture);

            return tmpSprite;
        }
    }
}
