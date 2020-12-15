using System.Collections.Generic;
using System.Linq;

namespace BombermanServer.Models.Flyweight
{
    public static class PlayerFlyweightFactory
    {
        private static readonly List<PlayerFlyweight> Flyweights = new List<PlayerFlyweight>();

        public static PlayerFlyweight GetPlayerFlyweight(PlayerSprite sprite)
        {
            var existingFlyweight = Flyweights.FirstOrDefault(fw => fw.Sprite == sprite);

            if (existingFlyweight is null)
            {
                var newFlyweight = new PlayerFlyweight(sprite);
                Flyweights.Add(newFlyweight);

                return newFlyweight;
            }

            return existingFlyweight;
        }
    }
}
