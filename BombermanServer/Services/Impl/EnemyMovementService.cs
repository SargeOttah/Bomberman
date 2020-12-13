using BombermanServer.Hubs;
using BombermanServer.Mediator;
using BombermanServer.Models;
using BombermanServer.Models.States.ConcreteStates;
using Microsoft.AspNetCore.SignalR;
using System.Drawing;
using System.Threading;

namespace BombermanServer.Services.Impl
{
    public class EnemyMovementService : IEnemyMovementService
    {
        private readonly IPlayerService _playerService;
        private readonly IMapService _mapService;
        private readonly IHubContext<UserHub> _hubContext;
        private readonly IPlayerDeathMediator _playerDeathMediator;

        private readonly Ghost _ghost;

        public EnemyMovementService(
            IHubContext<UserHub> hubContext,
            IPlayerService playerService,
            IMapService mapService,
            IPlayerDeathMediator playerDeathMediator)
        {
            _hubContext = hubContext;
            _playerService = playerService;
            _mapService = mapService;
            _playerDeathMediator = playerDeathMediator;

            _ghost = new Ghost { X = Ghost.StartingX, Y = Ghost.StartingY };
            _ghost.State = new InactiveGhostState(_ghost, playerService, mapService, playerDeathMediator);

            UpdateGhostMovement();

        }

        public void UpdateGhostMovement()
        {
            const int period = 300;

            var _ = new Timer(async _ =>
            {
                _ghost.UpdateState();
                _ghost.Move();

                var x = _ghost.X is null ? null : _ghost.X.ToString();
                var y = _ghost.Y is null ? null : _ghost.Y.ToString();

                await _hubContext.Clients.All.SendAsync("RefreshEnemies", x, y);
            }, null, 0, period);
        }

        public PointF GetGhostCoordinates()
        {
            if (_ghost.X is null || _ghost.Y is null) return new PointF(float.NaN, float.NaN);

            return new PointF(_ghost.X.Value, _ghost.Y.Value);
        }

        public void KillGhost()
        {
            _ghost.State = new DeadGhostState(_ghost, _playerService, _mapService, _playerDeathMediator);
        }
    }
}
