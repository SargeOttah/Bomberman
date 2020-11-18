using System;
using System.Collections.Generic;
using BombermanServer.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading;
using BombermanServer.Constants;

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
            _mapService.LoadMap();

            UpdateGhostMovement();
        }

        public void UpdateGhostMovement()
        {
            var x = 6.5f * MapConstants.tileSize;
            var y = 5.5f * MapConstants.tileSize;
            int? lastTurnIndex = null;
            var rnd = new Random();

            new Timer(async _ =>
            {
                if (_playerService.GetPlayers().Any())
                {
                    var allTurns = new List<bool>
                    {
                        !_mapService.IsObstacle(x - MapConstants.tileSize, y),
                        !_mapService.IsObstacle(x, y - MapConstants.tileSize),
                        !_mapService.IsObstacle(x + MapConstants.tileSize, y),
                        !_mapService.IsObstacle(x, y + MapConstants.tileSize)
                    };

                    var availableTurnIndexes = new List<int>();
                    for (var i = 0; i < allTurns.Count; i++)
                    {
                        if (allTurns[i]) availableTurnIndexes.Add(i);
                    }

                    if (lastTurnIndex is null || !allTurns[lastTurnIndex.Value])
                    {
                        lastTurnIndex = availableTurnIndexes[rnd.Next(availableTurnIndexes.Count)];
                    }

                    switch (lastTurnIndex)
                    {
                        case 0:
                            x -= MapConstants.tileSize;
                            break;
                        case 1: 
                            y -= MapConstants.tileSize;
                            break;
                        case 2: 
                            x += MapConstants.tileSize;
                            break;
                        case 3:
                            y += MapConstants.tileSize;
                            break;
                    }

                    await _hubContext.Clients.All.SendAsync("RefreshEnemies", x.ToString(), y.ToString());
                }
            }, null, 0, 100);
        }
    }
}
