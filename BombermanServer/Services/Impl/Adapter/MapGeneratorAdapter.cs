using System.Text;
using BombermanServer.Constants;

namespace BombermanServer.Services.Impl.Adapter
{
    public class MapGeneratorAdapter : IMapService
    {
        string currentName => nameof(MapGeneratorAdapter);
        MapGenerator mapGenerator;

        public MapGeneratorAdapter()
        {
            mapGenerator = new MapGenerator();
        }
        public void LoadMap(int id)
        {
            mapGenerator.FillEmptyMap();
            mapGenerator.AddObstacles();
            mapGenerator.ClearSpawnPoints();
        }

        public string[] GetMap()
        {
            return ConvertMap(mapGenerator.map);
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