using System.Threading;
using System.Drawing;
using BombermanServer.Models;
using BombermanServer.Constants;
using System;
using System.Text;
using System.Collections.Generic;

namespace BombermanServer.Services.Impl
{
    public class MapService : IMapService
    {
        string currentName => nameof(MapService);
        string[,] map;
        List<string> obstacleList;

        public MapService()
        {
            obstacleList = MapConstants.GetObstacleList();
        }

        public void LoadMap(int id) // load from file or whatever
        {
            Console.WriteLine("loading map");
            map = new string[9, 13]
            {
                { "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", },
                { "S", "2", "2", "2", "2", "2", "2", "2", "2", "2", "2", "2", "S", },
                { "S", "2", "O", "2", "S", "2", "O", "2", "S", "2", "O", "2", "S", },
                { "S", "2", "2", "2", "2", "2", "2", "2", "2", "2", "2", "2", "S", },
                { "S", "2", "O", "2", "S", "2", "O", "2", "S", "2", "O", "2", "S", },
                { "S", "2", "2", "B", "2", "2", "C", "2", "2", "2", "2", "2", "S", },
                { "S", "2", "O", "C", "S", "B", "O", "2", "S", "2", "O", "2", "S", },
                { "S", "2", "C", "B", "2", "2", "2", "2", "2", "2", "2", "2", "S", },
                { "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", },

            };
        }

        public string[] GetMap()
        {
            return ConvertMap(map);
        }

        public string[,] GetMapMatrix()
        {
            return map;
        }

        public Point GetTilePosition(float x, float y)
        {
            int tileX = (int)Math.Floor(x) / MapConstants.tileSize;
            int tileY = (int)Math.Floor(y) / MapConstants.tileSize;
            return new Point(tileX, tileY);
        }

        public bool IsObstacle(float x, float y)
        {
            var pos = GetTilePosition(x, y);
            return IsObstacle(pos.X, pos.Y);
        }

        public bool IsObstacle(int x, int y)
        {
            if (obstacleList.Contains(map[y, x]))
            {
                return true;
            }
            return false;
        }

        public void RemoveObstacle(int x, int y)
        {
            map[y, x] = ((char)TileType.Ground).ToString();
        }

        public string GetServiceName()
        {
            return currentName;
        }

        private string[] ConvertMap(string[,] map)
        {
            string[] convertedMap = new string[MapConstants.mapHeight];
            for (int i = 0; i < MapConstants.mapHeight; i++)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int j = 0; j < MapConstants.mapWidth; j++)
                {
                    stringBuilder.Append(map[i, j]);
                    if (j != MapConstants.mapWidth - 1) { stringBuilder.Append(","); }
                }
                convertedMap[i] = stringBuilder.ToString();
            }
            return convertedMap;
        }
    }
}
