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
            return mapGenerator.ConvertMap();
        }

        public string GetServiceName()
        {
            return currentName;
        }
    }
}