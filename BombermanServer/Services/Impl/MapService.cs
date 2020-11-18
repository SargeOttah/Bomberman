using BombermanServer.Models;
using BombermanServer.Constants;
using System;
using System.Collections.Generic;

namespace BombermanServer.Services.Impl
{
    public class MapService : IMapService
    {
        string currentName => nameof(MapService);
        string[] map;
        List<char> obstacleList;

        public MapService()
        {
            obstacleList = MapConstants.GetObstacleList();
        }

        public void LoadMap(int id) // load from file or whatever
        {
            map = new string[9]
            {
                "S,S,S,S,S,S,S,S,S,S,S,S,S",
                "S,2,2,2,2,2,2,2,2,2,2,2,S",
                "S,2,O,2,S,2,O,2,S,2,O,2,S",
                "S,2,2,2,2,2,2,2,2,2,2,2,S",
                "S,2,S,2,O,2,S,2,O,2,S,2,S",
                "S,2,2,2,2,2,2,2,2,2,2,2,S",
                "S,2,O,C,S,B,O,2,S,2,O,2,S",
                "S,2,2,2,2,2,2,2,2,2,2,2,S",
                "S,S,S,S,S,S,S,S,S,S,S,S,S"
            };
        }

        public string[] GetMap()
        {
            return map;
        }

        public bool IsObstacle(float x, float y)
        {
            int tileX = (int)Math.Floor(x) / MapConstants.tileSize * 2;
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
    }
}
