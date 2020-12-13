using BombermanServer.Models;
using BombermanServer.Services.Iterator;

namespace BombermanServer.Services.Impl
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerContainer _playerContainer;

        public PlayerService()
        {
            _playerContainer = new PlayerContainer();
        }

        public bool AddPlayer(Player player)
        {
            if (_playerContainer.GetCount() >= 4) { return false; }

            _playerContainer.AddPlayer(player);
            return true;
        }

        public Player GetPlayer(string connectionId)
        {
            var playerIterator = _playerContainer.GetIterator();

            while (playerIterator.HasNext())
            {
                var player = playerIterator.GetNext();

                if (player.ConnectionId.Equals(connectionId))
                {
                    return player;
                }
            }

            return null;
        }

        public Player GetPlayer(int id)
        {
            var playerIterator = _playerContainer.GetIterator();

            while (playerIterator.HasNext())
            {
                var player = playerIterator.GetNext();

                if (player.Id == id)
                {
                    return player;
                }
            }

            return null;
        }

        public int GetCount() => _playerContainer.GetCount();

        public IIterator GetPlayerIterator() => _playerContainer.GetIterator();

        public bool RemovePlayer(Player player) => _playerContainer.RemovePlayer(player);

        public int GetEmptyId()
        {
            var playerEmptyIdStrategy = PlayerServiceHelper.GetPlayerIdStrategy();

            return playerEmptyIdStrategy.GetEmptyId(_playerContainer.GetIterator());
        }

        public void KillPlayer(int id)
        {
            _playerContainer.KillPlayer(id);
        }
    }
}
