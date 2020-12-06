using BombermanServer.Models;
using System.Collections.Generic;

namespace BombermanServer.Services.Impl
{
    public class PlayerService : IPlayerService
    {
        private readonly List<Player> _players;

        public PlayerService()
        {
            _players = new List<Player>(4);
        }

        public bool AddPlayer(Player player)
        {
            if (_players.Count >= 4) { return false; }

            _players.Add(player);
            return true;
        }

        public Player GetPlayer(string connectionId)
        {
            return _players.Find(x => x.ConnectionId.Equals(connectionId));
        }

        public int GetCount()
        {
            return _players.Count;
        }

        public List<Player> GetPlayers()
        {
            return _players;
        }

        public bool RemovePlayer(Player player)
        {
            return _players.Remove(player);
        }
       
        public int GetEmptyId()
        {
            var playerEmptyIdStrategy = PlayerServiceHelper.GetPlayerIdStrategy();

            return playerEmptyIdStrategy.GetEmptyId(_players);
        }
    }
}
