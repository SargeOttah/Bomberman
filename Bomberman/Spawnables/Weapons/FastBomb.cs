using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Properties;
using Bomberman.Global;
using SFML.Graphics;

namespace Bomberman.Spawnables.Weapons
{
    class FastBomb : Bomb
    {
        // damage - placeDelay - bombTimer
        public FastBomb() : base(20, 500, (int)BombType.FastBomb) {
            this.ProjectileSprite = SpriteLoader.LoadSprite(Resources.fastbomb, new IntRect(0, 0, 64, 64));
            this.Origin = SpriteUtils.GetSpriteCenter(ProjectileSprite);
            //this.CurrentBombType = (int)BombType.FastBomb;
        }
    }
}
