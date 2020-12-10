using BombermanServer.Constants;
using BombermanServer.Mediator;
using BombermanServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BombermanServer.Models.States.ConcreteStates
{
    public class ActiveGhostState : GhostState
    {
        private readonly IPlayerService _playerService;
        private readonly IMapService _mapService;
        private readonly IPlayerDeathMediator _playerDeathMediator;

        public ActiveGhostState(Ghost context, IPlayerService playerService, IMapService mapService, IPlayerDeathMediator playerDeathMediator) : base(context)
        {
            _playerService = playerService;

            _mapService = mapService;
            _playerDeathMediator = playerDeathMediator;
            _mapService.LoadMap();
        }

        public override void Move()
        {
            var allTurns = GetAllTurns();
            var availableTurnIndexes = new List<int>(); // Indexes of LEGAL moves (if we can only go up or right, this list will contain numbers 1 and 2).
            for (var i = 0; i < allTurns.Count; i++)
            {
                if (allTurns[i]) availableTurnIndexes.Add(i);
            }

            if (LastTurnIndex is null || !allTurns[LastTurnIndex.Value]) // Critical need to recalculate new turn - either it's the first move of the ghost or our previous direction isn't legal anymore.
            {
                LastTurnIndex = availableTurnIndexes[RandomGenerator.Next(availableTurnIndexes.Count)];
            }
            else if (availableTurnIndexes.Count > 2) // If we recalculate only on the previous condition, we recalculate only in the corners of the border and ghost never enters the middle - this condition fixes that.
            {
                var newTurnIndex = availableTurnIndexes[RandomGenerator.Next(availableTurnIndexes.Count)];
                if (Math.Abs(LastTurnIndex.Value - newTurnIndex) != 2) // Unlike in previous condition, here we have the possibility to go back even though no obstacle was met - this could create wiggling and we never want that (this is why indexes of directions matter in allTurns!)
                {
                    LastTurnIndex = newTurnIndex;
                }
            }

            UpdatePosition();
            KillPlayers();
        }

        public override void UpdateState()
        {
            var players = _playerService.GetPlayers();

            if (!players.Any())
            {
                GhostContext.State = new InactiveGhostState(GhostContext, _playerService, _mapService, _playerDeathMediator);
            }
        }

        private void KillPlayers()
        {
            if (GhostContext.X is null || GhostContext.Y is null) return;
            var players = _playerService.GetPlayers();

            foreach (var player in players.Where(player => Math.Abs(GhostContext.X.Value - player.Position.X) < MapConstants.tileSize && Math.Abs(GhostContext.Y.Value - player.Position.Y) < MapConstants.tileSize))
            {
                _playerDeathMediator.Notify(player.Id);
            }
        }

        private void UpdatePosition()
        {
            switch (LastTurnIndex)
            {
                case 0:
                    GhostContext.X -= MapConstants.tileSize;
                    break;
                case 1:
                    GhostContext.Y -= MapConstants.tileSize;
                    break;
                case 2:
                    GhostContext.X += MapConstants.tileSize;
                    break;
                case 3:
                    GhostContext.Y += MapConstants.tileSize;
                    break;
            }
        }

        private List<bool> GetAllTurns() 
        {

            if (GhostContext.X is null || GhostContext.Y is null) return new List<bool>();

            return new List<bool>
            {
                !_mapService.IsObstacle(GhostContext.X.Value - MapConstants.tileSize, GhostContext.Y.Value),
                !_mapService.IsObstacle(GhostContext.X.Value, GhostContext.Y.Value - MapConstants.tileSize),
                !_mapService.IsObstacle(GhostContext.X.Value + MapConstants.tileSize, GhostContext.Y.Value),
                !_mapService.IsObstacle(GhostContext.X.Value, GhostContext.Y.Value + MapConstants.tileSize)
            };
        }
    }
}
