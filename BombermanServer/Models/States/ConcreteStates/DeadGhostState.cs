using BombermanServer.Mediator;
using BombermanServer.Services;
using System.Linq;

namespace BombermanServer.Models.States.ConcreteStates
{
    public class DeadGhostState : GhostState
    {
        private readonly IPlayerService _playerService;
        private readonly IMapService _mapService;
        private readonly IPlayerDeathMediator _playerDeathMediator;

        public DeadGhostState(Ghost context, IPlayerService playerService, IMapService mapService, IPlayerDeathMediator playerDeathMediator) : base(context)
        {
            _playerService = playerService;
            _mapService = mapService;
            _playerDeathMediator = playerDeathMediator;
        }

        public override void Move()
        {
            GhostContext.X = null;
            GhostContext.Y = null;
        }

        public override void UpdateState()
        {
            var players = _playerService.GetPlayers();

            if (!players.Any())
            {
                GhostContext.State = new InactiveGhostState(GhostContext, _playerService, _mapService, _playerDeathMediator);
            }
        }
    }
}
