namespace Bomberman.Map.Checks
{
    public class MapCheck
    {
        int screenSizeX;
        int screenSizeY;
        int tileSize;
        public MapCheck(int screenSizeX, int screenSizeY, int tileSize) {
            this.screenSizeX = screenSizeX;
            this.screenSizeY = screenSizeY;
            this.tileSize = tileSize;
        }
        // Checks if the provided map has enough defined tiles to cover the screen
        public bool IsMapValid(string[] map) {
            int tilesColumns = screenSizeX / tileSize;
            int tilesRows = screenSizeY / tileSize;
            return (map.Length >= tilesRows && map[0].Length >= tilesColumns);
        }
    }
}