using BombermanServer.Models;
using System.Collections.Generic;

namespace BombermanServer.Services.Impl
{
    public class PlayerService : IPlayerService
    {
        private readonly List<PlayerDTO> _players;

        public PlayerService()
        {
            _players = new List<PlayerDTO>(4);
        }

        public bool AddPlayer(PlayerDTO player)
        {
            if (_players.Count >= 4) { return false; }

            _players.Add(player);
            return true;
        }

        public PlayerDTO GetPlayer(string connectionId)
        {
            return _players.Find(x => x.ConnectionId.Equals(connectionId));
        }

        public int GetCount()
        {
            return _players.Count;
        }

        public List<PlayerDTO> GetPlayers()
        {
            return _players;
        }

        public bool RemovePlayer(PlayerDTO player)
        {
            return _players.Remove(player);
        }

        public int GetEmptyId()
        {
            var playerEmptyIdStrategy = PlayerServiceHelper.GetPlayerIdStrategy();

            return playerEmptyIdStrategy.GetEmptyId(_players);
        }

        public void KillPlayer(int id)
        {
            var playerIndex = _players.FindIndex(p => p.Id == id);

            if (playerIndex >= 0)
            {
                _players[playerIndex].IsDead = true;
            }
        }
    }
}
