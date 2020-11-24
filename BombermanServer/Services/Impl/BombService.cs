using BombermanServer.Hubs;
using BombermanServer.Models;
using BombermanServer.Constants;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BombermanServer.Services
{
    public class BombService : IBombService
    {
        private readonly int[,] directions = new int[4, 2]{
            { -1, 0 }, // left
            { 0, 1 }, // up
            { 1, 0 }, // right
            { 0, -1 }, // down
        };

        private List<Bomb> bombs;
        private List<string> destructableObstacles;
        private readonly IHubContext<UserHub> _hubContext;
        private IMapService mapService;
        public BombService(IHubContext<UserHub> hubContext, IMapService mapService)
        {
            bombs = new List<Bomb>();
            destructableObstacles = MapConstants.GetDestructableObstacles();
            _hubContext = hubContext;
            this.mapService = mapService;
        }

        public void Add(Bomb bomb)
        {
            SetBombExplosionTimer(bomb);
            bombs.Add(bomb);
            Console.WriteLine("bomb added");
            Console.WriteLine($"Bomb at: {bomb.Position.X} {bomb.Position.Y}");
        }

        public async void SetBombExplosionTimer(Bomb bomb)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(bomb.IgnitionDuration));
            Console.WriteLine("exploded");
            ExplodeBomb(bomb);
            await _hubContext.Clients.All.SendAsync("RefreshMap", mapService.GetMap());
            Console.WriteLine("PRINTING MAP:");
            var map = mapService.GetMap();

            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[0].Length; j++)
                {
                    Console.Write(map[i][j] + " ");
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }

        private void print(string[,] map)
        {
            int rowLength = map.GetLength(0);
            int colLength = map.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0} ", map[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }

        }

        public void ExplodeBomb(Bomb bomb)
        {
            var map = mapService.GetMapMatrix();
            var pos = mapService.GetTilePosition(bomb.Position.X, bomb.Position.Y);
            Console.WriteLine($"Bomb at: {pos.X} {pos.Y}");

            List<Point> tilesToRemove = new List<Point>();
            for (int i = 0; i < directions.GetLength(0); i++)
            {
                for (int j = 1; j < bomb.ExplosionRadius; j++)
                {
                    int x = pos.X + (directions[i, 0] * j);
                    int y = pos.Y + (directions[i, 1] * j);
                    Console.WriteLine($"explosion: {x} {y}");
                    if (x > MapConstants.mapWidth || x < 0)
                    {
                        break;
                    }
                    if (y > MapConstants.mapHeight || y < 0)
                    {
                        break;
                    }
                    Console.WriteLine(mapService.IsObstacle(x, y));
                    if (mapService.IsObstacle(x, y))
                    {
                        if (destructableObstacles.Contains(map[y, x]))
                        {
                            tilesToRemove.Add(new Point(x, y));
                        }
                        break;
                    }
                }
            }
            RemoveExplodedObstacles(tilesToRemove);
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