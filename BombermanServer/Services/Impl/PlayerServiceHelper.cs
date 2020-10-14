using BombermanServer.Services.Strategies;
using System;

namespace BombermanServer.Services.Impl
{
    public static class PlayerServiceHelper
    {
        public static IPlayerEmptyIdStrategy GetPlayerIdStrategy()
        {
            var rng = new Random();

            return rng.Next(0, 1) == 1
                ? new MaxPlayerEmptyIdStrategy() as IPlayerEmptyIdStrategy
                : new MinPlayerEmptyIdStrategy();
        }
    }
}
