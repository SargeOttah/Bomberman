using BombermanServer.Constants;
using BombermanServer.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
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
            _mapService.LoadMap();

            UpdateGhostMovement();
        }

        public void UpdateGhostMovement()
        {
            var x = 6.5f * MapConstants.tileSize; // Starting position (center of the board).
            var y = 5.5f * MapConstants.tileSize; 

            int? lastTurnIndex = null; // Direction we moved last turn.

            var rnd = new Random(); // RNG library for random movement.

            const int period = 300; // Time in ms between each recalculation.

            new Timer(async _ => // Infinite loop
            {
                var players = _playerService.GetPlayers();
                if (players.Any()) // Only do the calculations if we have any clients connected.
                {
                    var allTurns = new List<bool> // Check legality of each direction (false if the move is illegal, true if legal).
                    {
                        !_mapService.IsObstacle(x - MapConstants.tileSize, y), // Indexes matter - from 0 to 3 accordingly: left / up / right / down direction.
                        !_mapService.IsObstacle(x, y - MapConstants.tileSize),
                        !_mapService.IsObstacle(x + MapConstants.tileSize, y),
                        !_mapService.IsObstacle(x, y + MapConstants.tileSize)
                    };

                    var availableTurnIndexes = new List<int>(); // Indexes of LEGAL moves (if we can only go up or right, this list will contain numbers 1 and 2).
                    for (var i = 0; i < allTurns.Count; i++)
                    {
                        if (allTurns[i]) availableTurnIndexes.Add(i);
                    }

                    if (lastTurnIndex is null || !allTurns[lastTurnIndex.Value]) // Critical need to recalculate new turn - either it's the first move of the ghost or our previous direction isn't legal anymore.
                    {
                        lastTurnIndex = availableTurnIndexes[rnd.Next(availableTurnIndexes.Count)];
                    }
                    else if (availableTurnIndexes.Count > 2) // If we recalculate only on the previous condition, we recalculate only in the corners of the border and ghost never enters the middle - this condition fixes that.
                    {
                        var newTurnIndex = availableTurnIndexes[rnd.Next(availableTurnIndexes.Count)];
                        if (Math.Abs(lastTurnIndex.Value - newTurnIndex) != 2) // Unlike in previous condition, here we have the possibility to go back even though no obstacle was met - this could create wiggling and we never want that (this is why indexes of directions matter in allTurns!)
                        {
                            lastTurnIndex = newTurnIndex;
                        }
                    }

                    switch (lastTurnIndex) // Choose the direction based on (possibly) recalculated index (this is also why indexes matter).
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

                    Console.WriteLine(_playerService.GetPlayers().FirstOrDefault()?.Position.X);

                    await _hubContext.Clients.All.SendAsync("RefreshEnemies", x.ToString(), y.ToString());

                    foreach (var player in players)
                    {
                        if (
                            Math.Abs(x - player.Position.X) < MapConstants.tileSize  // check if ghost touched each player
                            && Math.Abs(y - player.Position.Y) < MapConstants.tileSize
                        )
                        {
                            await _hubContext.Clients.All.SendAsync("PlayerDied", player.ConnectionId);
                        }
                    }
                }
                else // Reset everything after players disconnect in order to have a clean start on another connection.
                {
                    x = 6.5f * MapConstants.tileSize; 
                    y = 5.5f * MapConstants.tileSize;

                    lastTurnIndex = null;
                }
            }, null, 0, period);
        }
    }
}
