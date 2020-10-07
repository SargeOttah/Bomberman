using BombermanServer.Models;
using System.Collections.Generic;

namespace BombermanServer.Services.Impl
{
    public class PlayerService : IPlayerService
    {
        List<Player> players;

        public PlayerService()
        {
            players = new List<Player>(4);
        }

        public bool AddPlayer(Player player)
        {
            if (players.Count >= 4) { return false; }

            players.Add(player);
            return true;
        }

        public Player GetPlayer(string connectionId)
        {
            return players.Find(x => x.ConnectionId.Equals(connectionId));
        }

        public int GetCount()
        {
            return players.Count;
        }

        public List<Player> GetPlayers()
        {
            return players;
        }

        public bool RemovePlayer(Player player)
        {
            return players.Remove(player);
        }
        // Returns the smallest not taken id
        public int GetFirstEmptyId()
        {
            for (int i = 0; i < 4; i++)
            {
                bool success = true;
                for (int j = 0; j < players.Count; j++)
                {
                    if (players[j].Id == i)
                    {
                        success = false;
                        break;
                    }
                }
                if (success) { return i; }
            }
            return -1;
        }
    }
}
