using BombermanServer.Hubs;
using BombermanServer.Models;
using BombermanServer.Constants;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using BombermanServer.Mediator;

namespace BombermanServer.Services
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
        public BombService(IHubContext<UserHub> hubContext, IMapService mapService, IPlayerService playerService, IPlayerDeathMediator playerDeathMediator)
        {
            bombs = new List<BombDTO>();
            destructableObstacles = MapConstants.GetDestructableObstacles();
            _hubContext = hubContext;
            this.mapService = mapService;
            _playerService = playerService;
            _playerDeathMediator = playerDeathMediator;
        }

        public void Add(BombDTO bomb) // TODO: maybe check if there already is a bomb on the tile?
        {
            var position = mapService.GetTilePosition(bomb.bombPosition.X, bomb.bombPosition.Y);
            bomb.bombPosition = new PointF(position.X * MapConstants.tileSize + MapConstants.tileSize / 2,
                                       position.Y * MapConstants.tileSize + MapConstants.tileSize / 2);
            bombs.Add(bomb);
            SetBombExplosionTimer(bomb);
            Console.WriteLine($"Bomb at: {bomb.bombPosition.X} {bomb.bombPosition.Y}");
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
            BombExplosion bombExplosion = new BombExplosion();
            bombExplosion.OwnerId = bomb.OwnerId;
            var map = mapService.GetMapMatrix();
            var pos = mapService.GetTilePosition(bomb.bombPosition.X, bomb.bombPosition.Y);

            List<Point> tilesToRemove = new List<Point>();
            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int x = pos.X;
                int y = pos.Y;
                for (int j = 1; j < bomb.ExplosionRadius; j++)
                {
                    x = pos.X + (directions[i, 0] * j);
                    y = pos.Y + (directions[i, 1] * j);

                    var players = _playerService.GetPlayers();
                    foreach (var player in players)
                    {
                        if (
                            Math.Abs(x - player.Position.X) > MapConstants.tileSize
                            && Math.Abs(y - player.Position.Y) > MapConstants.tileSize)
                        {
                            _playerDeathMediator.Notify(player.Id);
                        }
                    }

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