using BombermanServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BombermanServer.Services.Strategies
{
    public class MaxPlayerEmptyIdStrategy : IPlayerEmptyIdStrategy
    {
        public int GetEmptyId(IEnumerable<Player> players)
        {
            var tempId = 3;
            foreach (var player in players.OrderByDescending(p => p.Id))
            {
                if (player.Id != tempId)
                {
                    return tempId;
                }

                tempId--;
            }

            return -1;
        }
    }
}
