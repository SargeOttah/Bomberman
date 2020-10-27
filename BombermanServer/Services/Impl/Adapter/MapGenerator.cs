using System;
using BombermanServer.Constants;
using BombermanServer.Utils;

namespace BombermanServer.Services.Impl.Adapter
{
    public class MapGenerator
    {
        public string[,] map { get; private set; }

        public MapGenerator()
        {
            map = new string[MapConstants.mapHeight, MapConstants.mapWidth];
        }

        public void FillEmptyMap()
        {
            for (int i = 0; i < MapConstants.mapHeight; i++)
            {
                for (int j = 0; j < MapConstants.mapWidth; j++)
                {
                    if (i == 0 || i == MapConstants.mapHeight - 1 || j == 0 || j == MapConstants.mapWidth - 1)
                    {
                        map[i, j] = ((char)TileType.Stone).ToString(); // Adding borders
                    }
                    else
                    {
                        map[i, j] = ((char)TileType.Ground).ToString();
                    }
                }
            }
        }

        public void AddObstacles()
        {
            int width = MapConstants.mapWidth - 2;
            int height = MapConstants.mapHeight - 2;
            var data = new float[width * height];
            var min = float.MaxValue;
            var max = float.MinValue;

            Noise2d.Reseed();

            var frequency = 0.1f;
            var amplitude = 1f;

            for (int i = 0; i < data.Length; i++)
            {
                var x = i % width;
                var y = i / width;
                var noise = Noise2d.Noise(x * frequency * 1f / width, y * frequency * 1f / height);
                noise = data[y * width + x] += noise * amplitude;

                min = Math.Min(min, noise);
                max = Math.Max(max, noise);
            }

            for (int i = 1; i < MapConstants.mapHeight - 1; i++)
            {
                for (int j = 1; j < MapConstants.mapWidth - 1; j++)
                {
                    var normalizedNoise = (data[(i - 1) * (j - 1)] - min) / (max - min);
                    var obstacle = GetObstacleByNoise(normalizedNoise);
                    if (obstacle != null)
                    {
                        map[i, j] = obstacle;

                    }
                }
            }
        }

        public void ClearSpawnPoints()
        {
            map[1, 1] = ((char)TileType.Ground).ToString();
            map[MapConstants.mapHeight - 2, 1] = ((char)TileType.Ground).ToString();
            map[1, MapConstants.mapWidth - 2] = ((char)TileType.Ground).ToString();
            map[MapConstants.mapHeight - 2, MapConstants.mapWidth - 2] = ((char)TileType.Ground).ToString();
        }

        private string GetObstacleByNoise(float noise)
        {
            if (noise < 0.4f)
            {
                return null;
            }
            if (noise < 0.6f) { return ((char)TileType.Crate).ToString(); }
            if (noise < 0.8f) { return ((char)TileType.Brick).ToString(); }
            if (noise < 0.9f) { return ((char)TileType.Stone).ToString(); }
            return ((char)TileType.Obsidian).ToString();
        }
    }
}