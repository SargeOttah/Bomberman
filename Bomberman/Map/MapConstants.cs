using System;
using System.Linq;
using System.Collections.Generic;

namespace Bomberman.Map
{
    public class MapConstants
    {
        public static readonly int mapWidth = 13;
        public static readonly int mapHeight = 9;
        public static readonly int tileSize = 64;
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