namespace BombermanServer.Services
{
    public interface IMapService
    {
        public void LoadMap(int id = 0);
        public string[] GetMap();
        public bool IsObstacle(float x, float y);
        public string GetServiceName();

    }
}
