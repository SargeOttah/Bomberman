using BombermanServer.Constants;
using BombermanServer.Hubs;
using BombermanServer.Mediator;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace BombermanServer.Services.Impl
{
    public class EnemyMovementService : IEnemyMovementService
    {
        private readonly IPlayerService _playerService;
        private readonly IHubContext<UserHub> _hubContext;
        private readonly IMapService _mapService;
        private readonly IPlayerDeathMediator _playerDeathMediator;

        private float X = 6.5f * MapConstants.tileSize; // Starting position (center of the board).
        private float Y = 5.5f * MapConstants.tileSize;

        private bool isDead = false;

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
            _mapService.LoadMap();

            UpdateGhostMovement();
        }

        public void UpdateGhostMovement()
        {
            int? lastTurnIndex = null; // Direction we moved last turn.

            var rnd = new Random(); // RNG library for random movement.

            const int period = 300; // Time in ms between each recalculation.

            new Timer(async _ => // Infinite loop
            {
                var players = _playerService.GetPlayers();
                if (players.Any() && !isDead) // Only do the calculations if we have any clients connected and the ghost is alive.
                {
                    var allTurns = new List<bool> // Check legality of each direction (false if the move is illegal, true if legal).
                    {
                        !_mapService.IsObstacle(X - MapConstants.tileSize, Y), // Indexes matter - from 0 to 3 accordingly: left / up / right / down direction.
                        !_mapService.IsObstacle(X, Y - MapConstants.tileSize),
                        !_mapService.IsObstacle(X + MapConstants.tileSize, Y),
                        !_mapService.IsObstacle(X, Y + MapConstants.tileSize)
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
                            X -= MapConstants.tileSize;
                            break;
                        case 1: 
                            Y -= MapConstants.tileSize;
                            break;
                        case 2: 
                            X += MapConstants.tileSize;
                            break;
                        case 3:
                            Y += MapConstants.tileSize;
                            break;
                    }

                    await _hubContext.Clients.All.SendAsync("RefreshEnemies", X.ToString(), Y.ToString());

                    foreach (var player in players)
                    {
                        if (
                            Math.Abs(X - player.Position.X) < MapConstants.tileSize  // check if ghost touched each player
                            && Math.Abs(Y - player.Position.Y) < MapConstants.tileSize
                        )
                        {
                            _playerDeathMediator.Notify(player.Id);
                        }
                    }
                }
                else if (!players.Any()) // Reset everything after players disconnect in order to have a clean start on another connection.
                {
                    X = 6.5f * MapConstants.tileSize; 
                    Y = 5.5f * MapConstants.tileSize;

                    lastTurnIndex = null;
                    isDead = false;
                }
                else
                {
                    await _hubContext.Clients.All.SendAsync("RefreshEnemies", null, null);
                }
            }, null, 0, period);
        }

        public PointF GetGhostCoordinates()
        {
            return new PointF(X, Y);
        }

        public void KillGhost()
        {
            isDead = true;
        }
    }
}
