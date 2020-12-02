using System.Drawing;

namespace BombermanServer.Services
{
    public interface IMapService
    {
        public void LoadMap(int id = 0);
        public string[] GetMap();
        public string[,] GetMapMatrix();
        public bool IsObstacle(float x, float y);
        public bool IsObstacle(int x, int y);
        public void RemoveObstacle(int x, int y);
        public Point GetTilePosition(float x, float y);
        public string GetServiceName();

    }
}
