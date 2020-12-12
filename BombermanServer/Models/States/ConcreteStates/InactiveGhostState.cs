using BombermanServer.Mediator;
using BombermanServer.Services;

namespace BombermanServer.Models.States.ConcreteStates
{
    public class InactiveGhostState : GhostState
    {
        private readonly IPlayerService _playerService;
        private readonly IMapService _mapService;
        private readonly IPlayerDeathMediator _playerDeathMediator;

        public InactiveGhostState(Ghost context, IPlayerService playerService, IMapService mapService, IPlayerDeathMediator playerDeathMediator) : base(context)
        {
            _playerService = playerService;
            _mapService = mapService;
            _playerDeathMediator = playerDeathMediator;
        }

        public override void Move()
        {
            GhostContext.X = Ghost.StartingX;
            GhostContext.Y = Ghost.StartingY;

            LastTurnIndex = null;
        }

        public override void UpdateState()
        {
            var playerIterator = _playerService.GetPlayerIterator();

            if (playerIterator.HasNext())
            {
                Move();
                GhostContext.State = new ActiveGhostState(GhostContext, _playerService, _mapService, _playerDeathMediator);
            }
        }
    }
}
