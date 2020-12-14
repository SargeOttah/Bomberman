using System;
using System.Collections.Generic;
using System.Linq;

namespace BombermanServer.Constants
{
    public static class MapConstants
    {
        public static readonly int mapWidth = 13;
        public static readonly int mapHeight = 9;
        public static readonly int tileSize = 64;

        public static List<string> GetObstacleList()
        {
            List<string> obstacles = new List<string>();
            obstacles.AddRange(GetDestructableObstacles());
            obstacles.AddRange(GetUndestructableObstacles());
            return obstacles;
        }

        public static List<string> GetUndestructableObstacles()
        {
            List<string> obstacles = new List<string>();
            var undestructable = Enum.GetValues(typeof(UndestructableObstacles)).Cast<UndestructableObstacles>().Select(obstacle => ((char)obstacle).ToString());
            obstacles.AddRange(undestructable);
            return obstacles;
        }

        public static List<string> GetDestructableObstacles()
        {
            List<string> obstacles = new List<string>();
            var destructable = Enum.GetValues(typeof(DestructableObstacles)).Cast<DestructableObstacles>().Select(obstacle => ((char)obstacle).ToString());
            obstacles.AddRange(destructable);
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