using BombermanServer.Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace BombermanServer.Services.Impl.Adapter
{
    public class MapGeneratorAdapter : IMapService
    {
        string currentName => nameof(MapGeneratorAdapter);
        MapGenerator mapGenerator;
        string[] map;
        List<string> obstacleList;

        public MapGeneratorAdapter()
        {
            mapGenerator = new MapGenerator();
            obstacleList = MapConstants.GetObstacleList();
        }
        public void LoadMap(int id)
        {
            mapGenerator.FillEmptyMap();
            mapGenerator.AddObstacles();
            mapGenerator.ClearSpawnPoints();
            map = ConvertMap(mapGenerator.map);
        }

        public string[] GetMap()
        {
            return map;
        }

        public string[,] GetMapMatrix()
        {
            return mapGenerator.map;
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
            return obstacleList.Contains(mapGenerator.map[y, x]);
        }

        public void RemoveObstacle(int x, int y)
        {
            var line = map[y].ToCharArray();
            line[x] = (char)TileType.Ground;
            map[y] = new string(line);
        }

        public string GetServiceName()
        {
            return currentName;
        }

        private static string[] ConvertMap(string[,] map)
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