﻿using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using Bomberman.GUI.Visitor;
using Bomberman.GUI;

namespace Bomberman.Spawnables.Enemies
{
    public class Zombie : Enemy
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

        // Returns a shallow copy
        public override Enemy Clone()
        {
            return this.MemberwiseClone() as Enemy;
        }
    }
}
