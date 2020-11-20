using System;
using System.Linq;
using System.Collections.Generic;

namespace BombermanServer.Constants
{
    public static class MapConstants
    {
        public static readonly int mapWidth = 13;
        public static readonly int mapHeight = 9;
        public static readonly int tileSize = 64;

        public static List<char> GetObstacleList()
        {
            List<char> obstacles = new List<char>();
            var destructable = Enum.GetValues(typeof(DestructableObstacles)).Cast<DestructableObstacles>().Select(obstacle => (char)obstacle);
            var undestructable = Enum.GetValues(typeof(UndestructableObstacles)).Cast<UndestructableObstacles>().Select(obstacle => (char)obstacle);
            obstacles.AddRange(destructable);
            obstacles.AddRange(undestructable);
            return obstacles;
        }
    }

    public enum TileType
    {
        Ground = '2',
        Stone = 'S',
        Brick = 'B',
        Obsidian = 'O',
        Crate = 'C',
    }

    public enum DestructableObstacles
    {
        Brick = 'B',
        Crate = 'C',
    }

    public enum UndestructableObstacles
    {
        Stone = 'S',
        Obsidian = 'O'
    }
}