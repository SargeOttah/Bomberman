using BombermanServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BombermanServer.Services.Iterator
{
    public class PlayerContainer : IPlayerContainer
    {
        private List<PlayerDTO> _players;

        public PlayerContainer()
        {
            _players = new List<PlayerDTO>();
        }

        public IIterator GetIterator() => new PlayerIterator(_players);

        public void AddPlayer(PlayerDTO player)
        { 
            _players.Add(player);
        }

        public int GetCount() => _players.Count;

        public bool RemovePlayer(PlayerDTO player)
        {
            return _players.Remove(player);
        }

        public void KillPlayer(int id)
        {
            var player = _players.First(p => p.Id == id);

            player.IsDead = true;
        }
    }
}
