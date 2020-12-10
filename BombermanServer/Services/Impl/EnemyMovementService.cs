using BombermanServer.Constants;
using BombermanServer.Hubs;
using BombermanServer.Mediator;
using BombermanServer.Models;
using BombermanServer.Models.States.ConcreteStates;
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

        private Ghost Ghost;

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

            Ghost = new Ghost { Position = new PointF(6.5f * MapConstants.tileSize, 5.5f * MapConstants.tileSize) };
            var ghostState = new InactiveGhostState(Ghost);
            Ghost.State = ghostState;
        }

        public void UpdateGhostMovement()
        {
            const int period = 300;

            new Timer(async _ =>
            {
                var players = _playerService.GetPlayers();
                if (players.Any() && Ghost.State is InactiveGhostState) Ghost.State = new ActiveGhostState(Ghost);
                else if (!players.Any() && Ghost.State is ActiveGhostState) Ghost.State = new InactiveGhostState(Ghost);
                
                var allTurns = new List<bool> 
                {
                    !_mapService.IsObstacle(Ghost.Position.X - MapConstants.tileSize, Ghost.Position.Y),
                    !_mapService.IsObstacle(Ghost.Position.X, Ghost.Position.Y - MapConstants.tileSize),
                    !_mapService.IsObstacle(Ghost.Position.X + MapConstants.tileSize, Ghost.Position.Y),
                    !_mapService.IsObstacle(Ghost.Position.X, Ghost.Position.Y + MapConstants.tileSize)
                };
                Ghost.Move(allTurns);
                Console.WriteLine(Ghost.Position.X);
                await _hubContext.Clients.All.SendAsync("RefreshEnemies", Ghost.Position.X, Ghost.Position.Y); 
                foreach (var player in players.Where(player => Math.Abs(Ghost.Position.X - player.Position.X) < MapConstants.tileSize && Math.Abs(Ghost.Position.Y - player.Position.Y) < MapConstants.tileSize))
                {
                    _playerDeathMediator.Notify(player.Id);
                }
            }, null, 0, period);
        }

        public PointF GetGhostCoordinates() => Ghost.Position;

        public void KillGhost()
        {
            Ghost.State = new DeadGhostState(Ghost);
        }
    }
}
