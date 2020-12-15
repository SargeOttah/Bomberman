using BombermanServer.Services;

namespace BombermanServer.Mediator
{
    public class PlayerDeathMediator : IPlayerDeathMediator
    {
        private readonly IPlayerService _playerService;

        public PlayerDeathMediator(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public void Notify(int playerId)
        {
            _playerService.KillPlayer(playerId);
        }
    }
}
