using Bomberman.Spawnables.Obstacles;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
//https://github.com/SFML/SFML/wiki/Source:-TileMap-Render

namespace Bomberman.Map
{
    public class TileMap : Drawable
    {
        private readonly int tileSize;
        private readonly int spriteSize;

        private string[,] map;

        private Vector2i offset;
        private List<Tile> tiles;
        private List<Obstacle> obstacles;

        private Texture spriteSheet;

        private int height;
        private int width;

        /// <param name="tileSize">Pixel size of the tile that is going to be rendered</param>
        /// <param name="spriteSize">Pixel size of the sprite sizes in sprite sheet</param>
        public TileMap(Texture spriteSheet, string[] map, int tileSize = 64, int spriteSize = 8)
        {
            this.tiles = new List<Tile>();
            this.spriteSheet = spriteSheet;
            this.tileSize = tileSize;
            this.spriteSize = spriteSize;
            ParseMap(map);
        }

        private void ParseMap(string[] map)
        {
            this.map = new string[map.Length, map[0].Split(",").Length];

            for (int i = 0; i < map.Length; i++)
            {
                string[] values = map[i].Split(",");
                for (int j = 0; j < values.Length; j++)
                {
                    this.map[i, j] = values[j];
                    Console.WriteLine(values[j]);
                }
            }
        }


        /// <summary>
        /// Redraws whole screen
        /// </summary>
        public void Refresh()
        {
            RefreshLocal(0, 0, width, height);
        }

        private void RefreshLocal(int left, int top, int right, int bottom)
        {
            for (var y = top; y < bottom; y++)
                for (var x = left; x < right; x++)
                {
                    Refresh(x + offset.X, y + offset.Y);
                }
        }

        /// <summary>
        /// Ensures that vertex array has enough space
        /// </summary>
        /// <param name="v">Size of the visible area</param>
        private void SetSize(Vector2f v)
        {
            var w = (int)(v.X / tileSize) + 2;
            var h = (int)(v.Y / tileSize) + 2;
            //Console.WriteLine($"screen Width: {v.X}, Height: {v.Y}");

            if (w == width && h == height) return;
            //Console.WriteLine($"Width: {w}, Height: {h}");
            // First time initialization
            width = w;
            height = h;

            tiles = new List<Tile>(width * height);
            obstacles = new List<Obstacle>(width * height);

            LoadTiles(0, 0);
            Refresh();
        }

        private void LoadTiles(int x, int y)
        {
            ObstacleFactory destroyableFactory = FactoryPicker.GetFactory("Destroyable");
            ObstacleFactory undestroyableFactory = FactoryPicker.GetFactory("Undestroyable");

            for (int i = y; i < height; i++)
            {
                for (int j = x; j < width; j++)
                {
                    int texture = 0; // Default ground tile
                    // If we launch the game and map is not big enough it will throw an npe XD
                    switch (map[i, j])
                    {
                        // Destructible Obstacles
                        case "B":
                            Console.WriteLine("Adding brick wall" + j + " " + i);
                            obstacles.Add(destroyableFactory.GetDestroyable("BrickWall"));
                            break;
                        case "C":
                            obstacles.Add(destroyableFactory.GetDestroyable("Crate"));
                            break;
                        // Undestructible Obstacles
                        case "O":
                            obstacles.Add(undestroyableFactory.GetUndestroyable("Obsidian"));
                            break;
                        case "S":
                            obstacles.Add(undestroyableFactory.GetUndestroyable("Stone"));
                            break;
                        default:
                            if (int.TryParse(map[i, j], out texture))
                            {
                                obstacles.Add(null);
                            }
                            else
                            {
                                throw new ArgumentException($"Map tile {map[i, j]} does not map to any object.");
                            }
                            break;
                    }
                    tiles.Add(new Tile(texture));
                }
            }
        }

        /// <summary>
        /// Sets offset
        /// </summary>
        /// <param name="v">World position of top left corner of the screen</param>
        private void SetCorner(Vector2f v)
        {
            var tile = GetTile(v);
            var dif = tile - offset;
            if (dif.X == 0 && dif.Y == 0) return;
            offset = tile;

            if (Math.Abs(dif.X) > width / 4 || Math.Abs(dif.Y) > height / 4)
            {
                //Refresh everyting if difference is too big
                Refresh();
                return;
            }

            //Refresh only tiles that appeared since last update

            if (dif.X > 0) RefreshLocal(width - dif.X, 0, width, height);
            else RefreshLocal(0, 0, -dif.X, height);

            if (dif.Y > 0) RefreshLocal(0, height - dif.Y, width, height);
            else RefreshLocal(0, 0, width, -dif.Y);
        }

        /// <summary>
        /// Transforms from world size to to tile that is under that position
        /// </summary>
        private Vector2i GetTile(Vector2f pos)
        {
            var x = (int)(pos.X / tileSize);
            var y = (int)(pos.Y / tileSize);
            if (pos.X < 0) x--;
            if (pos.Y < 0) y--;
            return new Vector2i(x, y);
        }

        /// <summary>
        /// Redraws one tile
        /// </summary>
        /// <param name="x">X coord of the tile</param>
        /// <param name="y">Y coord of the tile</param>
        public void Refresh(int x, int y)
        {
            if (x < offset.X || x >= offset.X + width || y < offset.Y || y >= offset.Y + height)
                return; //check if tile is visible

            //vertices works like 2d ring buffer
            var vx = x % width;
            var vy = y % height;
            if (vx < 0) vx += width;
            if (vy < 0) vy += height;

            bool isObstacle = !int.TryParse(map[vy, vx], out _);
            var index = vx + vy * width;
            var rec = new FloatRect(x * tileSize, y * tileSize, tileSize, tileSize);

            int textureIdx = isObstacle ? obstacles[index].tileIndex : tiles[index].tileIndex;
            var textureX = (textureIdx * spriteSize) % width;
            var textureY = (textureIdx * spriteSize) % height;
            IntRect src = new IntRect(textureX, textureY, spriteSize, spriteSize);

            
            if (!isObstacle)
            {
                tiles[index].UpdateTile(rec, src);
            } else
            {
                if (obstacles[index] != null) // this shouldn't happen, buuuuut just for safety
                {
                    obstacles[index].UpdateTile(rec, src);
                }
            }

            //Console.WriteLine($"vx {vx} {vy} {width}");
            
        }


        /// <summary>
        /// Update offset (based on Target's position) and draw it
        /// </summary>
        public void Draw(RenderTarget rt, RenderStates states)
        {
            var view = rt.GetView();
            states.Texture = spriteSheet;

            SetSize(view.Size);
            SetCorner(rt.MapPixelToCoords(new Vector2i()));

            foreach (Tile tile in tiles)
            {
                rt.Draw(tile.vertices, PrimitiveType.Quads, states);
            }

            // Drawing obstacles on top of ground
            foreach (Obstacle obstacle in obstacles)
            {
                if (obstacle != null)
                {
                    rt.Draw(obstacle.vertices, PrimitiveType.Quads, states);
                }
            }
            
        }
    }
}
