using System;
using BombermanServer.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading;

namespace BombermanServer.Services.Impl
{
    public class EnemyMovementService : IEnemyMovementService
    {
        private readonly IPlayerService _playerService;
        private readonly IHubContext<UserHub> _hubContext;
        private readonly IMapService _mapService;

        public EnemyMovementService(
            IHubContext<UserHub> hubContext,
            IPlayerService playerService,
            IMapService mapService)
        {
            _hubContext = hubContext;
            _playerService = playerService;

            _mapService = mapService;

            UpdateGhostMovement();
        }

        public void UpdateGhostMovement()
        {
            var x = 0;
            var y = 480;
            new Timer(async _ =>
            {
                var map = _mapService.GetMap();
                foreach (var m in map)
                {
                    Console.WriteLine(m);
                }
                if (_playerService.GetPlayers().Any())
                {
                    x += 10;
                    await _hubContext.Clients.All.SendAsync("RefreshEnemies", x.ToString(), y.ToString());
                }
            }, null, 0, 100);
        }
    }
}
