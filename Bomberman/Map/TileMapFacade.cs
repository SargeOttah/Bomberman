using System;
using SFML.Graphics;
using Bomberman.Map.Checks;

namespace Bomberman.Map {
    public class TileMapFacade {

        int tileSize;
        int spriteSize;

        int screenSizeX;
        int screenSizeY;

        Texture spriteSheet;

        public TileMap tileMap {get; private set;}

        MapCheck mapChecker;
        SpriteSheetCheck spriteSheetChecker;

        public TileMapFacade(int screenSizeX, int screenSizeY,  byte[] spriteSheet, int tileSize = 64, int spriteSize = 32) {
            this.screenSizeX = screenSizeX;
            this.screenSizeY = screenSizeY;
            this.tileSize = tileSize;
            this.spriteSize = spriteSize;
            this.spriteSheet = new Texture(spriteSheet);
            mapChecker = new MapCheck(screenSizeX, screenSizeY, tileSize);
            spriteSheetChecker = new SpriteSheetCheck(spriteSize);
        }
        
        public void UpdateTileMap(string[] map) {
            if (!ValidateMap(map)) {
                Console.WriteLine("Invalid map: " + map.ToString());
                return;
            }

            tileMap.ParseMap(map);
        }

        public bool SetupTileMap(string[] map) {
            if (spriteSheetChecker.IsValidSpriteSheet(spriteSheet) && ValidateMap(map)) {
                Console.WriteLine("Creating tilemap");
                tileMap = new TileMap(spriteSheet, map);
                return true;
            }
            return false;
        }

        public TileMap GetTileMap(){
            return tileMap;
        }

        private bool ValidateMap(string[] map) {
            if (!mapChecker.IsMapValid(map)) {
                Console.WriteLine("Map size is invalid.");
                return false;
            }
            for (int i = 0; i < map.Length; i++) {
                for (int j = 0; j < map[0].Length; j++) {
                    if (!spriteSheetChecker.IsValidTile(map[i][j], spriteSheet)) { 
                        Console.WriteLine($"Tile: {map[i][j]} is invalid.");
                        return false; 
                    }
                }
            }
            return true;
        }
    }
}