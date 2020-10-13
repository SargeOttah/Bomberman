using BombermanServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BombermanServer.Services.Strategies
{
    public class MinPlayerEmptyIdStrategy : IPlayerEmptyIdStrategy
    {
        public int GetEmptyId(IEnumerable<Player> players)
        {
            var tempId = 0;
            foreach (var player in players.OrderBy(p => p.Id))
            {
                if (player.Id != tempId)
                {
                    return tempId;
                }

                tempId++;
            }

            return -1;
        }
    }
}
