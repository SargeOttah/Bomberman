using Bomberman.Spawnables.Obstacles;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Bomberman.Spawnables.Tiles;

//https://github.com/SFML/SFML/wiki/Source:-TileMap-Render

namespace Bomberman.Map
{
    public class TileMap : Drawable
    {
        private readonly int tileSize;
        private readonly int spriteSize;

        private string[,] map;

        private Vector2i offset;
        private List<Ground> tiles;
        private List<Obstacle> obstacles;
        private CompoundTile _compoundTile;

        private static Texture _spriteSheet;

        private int height;
        private int width;

        /// <param name="tileSize">Pixel size of the tile that is going to be rendered</param>
        /// <param name="spriteSize">Pixel size of the sprite sizes in sprite sheet</param>
        public TileMap(Texture spriteSheet, string[] map, int tileSize = 64, int spriteSize = 32)
        {
            TileMap._spriteSheet = spriteSheet;
            this.tiles = new List<Ground>();
            this._compoundTile = new CompoundTile();
            this.tileSize = tileSize;
            this.spriteSize = spriteSize;
            ParseMap(map);
        }

        public static Texture GetSpriteSheet()
        {
            return _spriteSheet;
        }

        public void ParseMap(string[] map)
        {
            this.map = new string[map.Length, map[0].Split(",").Length];

            for (int i = 0; i < map.Length; i++)
            {
                string[] values = map[i].Split(",");
                for (int j = 0; j < values.Length; j++)
                {
                    this.map[i, j] = values[j];
                }
            }
        }

        public void UpdateMap(string[] map)
        {
            List<Point> tilesToUpdate = new List<Point>();
            for (int i = 0; i < map.Length; i++)
            {
                string[] values = map[i].Split(",");
                for (int j = 0; j < values.Length; j++)
                {
                    if (!this.map[i, j].Equals(values[j]))
                    {
                        Console.WriteLine($"deleting tile at: {j + i * width}");
                        obstacles[j + i * width] = null;
                        tilesToUpdate.Add(new Point(j, i));
                    }
                }
            }
            ParseMap(map);

            foreach (var tile in tilesToUpdate)
            {
                Refresh(tile.X, tile.Y);
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
            var w = (int)(v.X / tileSize);
            var h = (int)(v.Y / tileSize);
            _compoundTile = new CompoundTile();
            //Console.WriteLine($"screen Width: {v.X}, Height: {v.Y}");

            if (w == width && h == height) return;
            //Console.WriteLine($"Width: {w}, Height: {h}");
            // First time initialization
            width = w;
            height = h;

            tiles = new List<Ground>(width * height);
            obstacles = new List<Obstacle>(width * height);

            LoadTiles(0, 0);
            Refresh();
        }

        private void LoadTiles(int x, int y)
        {
            ObstacleFactory destroyableFactory = FactoryPicker.GetFactory("Destroyable");
            ObstacleFactory undestroyableFactory = FactoryPicker.GetFactory("Undestroyable");
            var index = 0;

            for (int i = y; i < height; i++)
            {
                for (int j = x; j < width; j++)
                {
                    int texture = 2; // Default ground tile
                    // If we launch the game and map is not big enough it will throw an npe XD
                    switch (map[i, j])
                    {
                        // Destructible Obstacles
                        case "B":
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
                    // We add a ground tile every time

                    tiles.Add(new Ground(texture));

                    //_compoundTile.Add(obstacles[index] == null ? tiles[index] : obstacles[index]);
                }
            }
        }
        // Returns only those obstacles that are close to pos
        public List<Obstacle> GetCloseObstacles(Vector2f pos)
        {
            var tile = GetTile(pos);
            return obstacles.FindAll((obs) => obs != null); // TODO: finish lol
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
                //Refresh everything if difference is too big
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

            int textureIdx = isObstacle ? obstacles[index].TileIndex : tiles[index].TileIndex;
            var textureX = (textureIdx * spriteSize) % unchecked((int)_spriteSheet.Size.X);
            var textureY = textureIdx / (unchecked((int)_spriteSheet.Size.X) / spriteSize) * spriteSize;
            //Console.WriteLine($"texturing {x} {y} {textureIdx} {textureX} {textureY} {spriteSize}");
            IntRect src = new IntRect(textureX, textureY, spriteSize, spriteSize);

            if (!isObstacle)
            {
                tiles[index].UpdateTile(rec, src);
            }
            else
            {
                if (obstacles[index] != null) // this shouldn't happen, buuuuut just for safety
                {
                    obstacles[index].UpdateTile(rec, src);
                }
            }

            //Console.WriteLine($"vx {vx} {vy} {width}");
        }

        private void FillCompoundTile()
        {
            foreach (var tile in tiles)
            {
                _compoundTile.Add(tile);
            }

            foreach (var obstacle in obstacles.Where(obstacle => obstacle != null))
            {
                _compoundTile.Add(obstacle);
            }
        }


        /// <summary>
        /// Update offset (based on Target's position) and draw it
        /// </summary>
        public void Draw(RenderTarget rt, RenderStates states)
        {
            
            var view = rt.GetView();
            states.Texture = _spriteSheet;

            SetSize(view.Size);
            SetCorner(rt.MapPixelToCoords(new Vector2i()));
            FillCompoundTile();

            var vertices = _compoundTile.GetVertices();
            for (int i = 0; i < vertices.Length; i += 4)
            {
                rt.Draw(GetRange(vertices, i, 4), PrimitiveType.Quads, states);
            }
        }

        public Vertex[] GetRange(Vertex[] array, int index, int count)
        {
            var newArray = new Vertex[count];
            if (array == null || array.Length < index + count) return newArray;

            for (var i = index; i < index + count; i++)
            {
                newArray[i - index] = array[i];
            }

            return newArray;
        }
    }
}
