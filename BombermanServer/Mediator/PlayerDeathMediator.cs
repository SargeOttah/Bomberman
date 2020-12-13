using BombermanServer.Services;

namespace BombermanServer.Mediator
{
    public class PlayerDeathMediator : IPlayerDeathMediator
    {
        private readonly IPlayerService _playerService;
        private readonly IRespawnService _respawnService;

        public PlayerDeathMediator(IPlayerService playerService, IRespawnService respawnService)
        {
            _playerService = playerService;
            _respawnService = respawnService;
        }

        public void Notify(int playerId)
        {
            _playerService.KillPlayer(playerId);
            _respawnService.QueueRespawn(_playerService.GetPlayer(playerId)._snapshot);
        }
    }
}
