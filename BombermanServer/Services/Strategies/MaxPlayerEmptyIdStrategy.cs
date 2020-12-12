using BombermanServer.Models;
using BombermanServer.Services.Iterator;
using System.Collections.Generic;
using System.Linq;

namespace BombermanServer.Services.Strategies
{
    public class MaxPlayerEmptyIdStrategy : IPlayerEmptyIdStrategy
    {
        public int GetEmptyId(IIterator playerIterator)
        {
            var players = new List<PlayerDTO>();
            while (playerIterator.HasNext())
            {
                players.Add(playerIterator.GetNext());
            }

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
