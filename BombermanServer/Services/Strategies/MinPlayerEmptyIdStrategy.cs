using BombermanServer.Models;
using BombermanServer.Services.Iterator;
using System.Collections.Generic;
using System.Linq;

namespace BombermanServer.Services.Strategies
{
    public class MinPlayerEmptyIdStrategy : IPlayerEmptyIdStrategy
    {
        public int GetEmptyId(IIterator playerIterator)
        {
            var players = new List<PlayerDTO>();
            while (playerIterator.HasNext())
            {
                players.Add(playerIterator.GetNext());
            }

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
