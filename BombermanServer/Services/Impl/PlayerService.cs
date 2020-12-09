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
       
        public int GetEmptyId()
        {
            var playerEmptyIdStrategy = PlayerServiceHelper.GetPlayerIdStrategy();

            return playerEmptyIdStrategy.GetEmptyId(players);
        }

        public void KillPlayer(int id)
        {
            var playerIndex = players.FindIndex(p => p.Id == id);

            if (playerIndex >= 0)
            {
                players[playerIndex].IsDead = true;
            }
        }
    }
}
