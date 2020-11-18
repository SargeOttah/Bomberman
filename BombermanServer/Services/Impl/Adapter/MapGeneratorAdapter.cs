using System;
using System.Text;
using BombermanServer.Constants;
using System.Collections.Generic;

namespace BombermanServer.Services.Impl.Adapter
{
    public class MapGeneratorAdapter : IMapService
    {
        string currentName => nameof(MapGeneratorAdapter);
        MapGenerator mapGenerator;
        string[] map;
        List<char> obstacleList;

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

        public bool IsObstacle(float x, float y)
        {
            int tileX = (int)Math.Floor(x) / MapConstants.tileSize;
            int tileY = (int)Math.Floor(y) / MapConstants.tileSize;

            if (obstacleList.Contains(map[tileY][tileX]))
            {
                return true;
            }
            return false;
        }

        public string GetServiceName()
        {
            return currentName;
        }

        public string[] ConvertMap(string[,] map)
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