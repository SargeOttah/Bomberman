﻿using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Properties;
using Bomberman.Global;
using SFML.Graphics;
using Bomberman.Dto;

namespace Bomberman.Spawnables.Weapons
{
    class SuperBomb : Bomb
    {
        // damage - placeDelay - bombTimer
        public SuperBomb() : base(20, 2000, (int)BombType.SuperBomb) {
            this.ProjectileSprite = SpriteLoader.LoadSprite(Resources.superbomb, new IntRect(0, 0, 64, 64));
            this.Origin = SpriteUtils.GetSpriteCenter(ProjectileSprite);
            //this.CurrentBombType = (int)BombType.SuperBomb;
        }
    }
}
