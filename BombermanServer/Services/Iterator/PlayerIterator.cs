using BombermanServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BombermanServer.Services.Iterator
{
    public class PlayerIterator : IIterator
    {
        private readonly List<Player> _players;

        private int _index;

        public PlayerIterator(IEnumerable<Player> players)
        {
            _players = players.ToList();
        }

        public Player GetNext()
        {
            _index++;

            return _players.ElementAt(_index - 1);
        }

        public bool HasNext() => _index < _players.Count;
    }
}
