using BombermanServer.Constants;
using BombermanServer.Hubs;
using BombermanServer.Mediator;
using BombermanServer.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace BombermanServer.Services.Impl
{
    public class BombService : IBombService
    {
        private readonly int[,] directions = new int[4, 2]{
            { -1, 0 }, // left
            { 0, 1 }, // down
            { 1, 0 }, // right
            { 0, -1 }, // up
        };

        private List<BombDTO> bombs;
        private List<string> destructableObstacles;
        private readonly IHubContext<UserHub> _hubContext;
        private IMapService mapService;
        private IPlayerService _playerService;
        private IPlayerDeathMediator _playerDeathMediator;
        private readonly IEnemyMovementService _enemyMovementService;
        public BombService(IHubContext<UserHub> hubContext, IMapService mapService, IPlayerService playerService, IPlayerDeathMediator playerDeathMediator, IEnemyMovementService enemyMovementService)
        {
            bombs = new List<BombDTO>();
            destructableObstacles = MapConstants.GetDestructableObstacles();
            _hubContext = hubContext;
            this.mapService = mapService;
            _playerService = playerService;
            _playerDeathMediator = playerDeathMediator;
            _enemyMovementService = enemyMovementService;
        }

        public void Add(BombDTO bomb) // TODO: maybe check if there already is a bomb on the tile?
        {
            var position = mapService.GetTilePosition(bomb.Position.X, bomb.Position.Y);
            bomb.Position = new PointF(position.X * MapConstants.tileSize + MapConstants.tileSize / 2,
                                       position.Y * MapConstants.tileSize + MapConstants.tileSize / 2);
            bombs.Add(bomb);
            SetBombExplosionTimer(bomb);
            Console.WriteLine($"Bomb at: {bomb.Position.X} {bomb.Position.Y}");
        }

        public List<BombDTO> GetBombs()
        {
            return bombs;
        }

        private bool RemoveBomb(BombDTO bomb)
        {
            return bombs.Remove(bomb);
        }

        public async void SetBombExplosionTimer(BombDTO bomb)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(bomb.IgnitionDuration));
            ExplodeBomb(bomb);
            await _hubContext.Clients.All.SendAsync("RefreshMap", mapService.GetMap());
        }

        public void ExplodeBomb(BombDTO bomb)
        {
            BombExplosion bombExplosion = new BombExplosion {OwnerId = bomb.OwnerId};
            var map = mapService.GetMapMatrix();
            var pos = mapService.GetTilePosition(bomb.Position.X, bomb.Position.Y);

            List<Point> tilesToRemove = new List<Point>();
            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int x = pos.X;
                int y = pos.Y;
                for (int j = 1; j < bomb.ExplosionRadius; j++)
                {
                    x = pos.X + (directions[i, 0] * j);
                    y = pos.Y + (directions[i, 1] * j);

                    if (x > MapConstants.mapWidth || x < 0)
                    {
                        break;
                    }
                    if (y > MapConstants.mapHeight || y < 0)
                    {
                        break;
                    }
                    if (mapService.IsObstacle(x, y))
                    {
                        if (destructableObstacles.Contains(map[y, x]))
                        {
                            tilesToRemove.Add(new Point(x, y));
                        }
                        break;
                    }

                    var playerIterator = _playerService.GetPlayerIterator();
                    while (playerIterator.HasNext())
                    {
                        var player = playerIterator.GetNext();
                        if (
                            Math.Abs(x - Math.Floor(player.Position.X / MapConstants.tileSize)) == 0
                            && Math.Abs(y - Math.Floor(player.Position.Y / MapConstants.tileSize)) == 0)
                        {
                            _playerDeathMediator.Notify(player.Id);
                        }
                    }

                    var ghostCoordinates = _enemyMovementService.GetGhostCoordinates();
                    if (
                        Math.Abs(x - Math.Floor(ghostCoordinates.X / MapConstants.tileSize)) == 0
                        && Math.Abs(y - Math.Floor(ghostCoordinates.Y / MapConstants.tileSize)) == 0)
                    {
                        _enemyMovementService.KillGhost();
                    }
                }
                bombExplosion.ExplosionCoords[i] = new Point(x, y);
            }
            SendExplosionEvent(bombExplosion).Wait();
            RemoveExplodedObstacles(tilesToRemove);
            RemoveBomb(bomb);
        }

        private async Task SendExplosionEvent(BombExplosion bombExplosion)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNewExplosion", bombExplosion);
        }

        private void RemoveExplodedObstacles(List<Point> tiles)
        {
            foreach (Point tile in tiles)
            {
                Console.WriteLine($"removing obstacle: {tile.X} {tile.Y}");
                mapService.RemoveObstacle(tile.X, tile.Y);
            }
        }
    }
}