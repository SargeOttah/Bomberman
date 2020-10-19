using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;
//https://github.com/SFML/SFML/wiki/Source:-TileMap-Render

namespace Bomberman.Map
{
    public class TileMap : Drawable
    {
        private readonly int tileSize;

        private readonly int[,] map;

        private Vector2i offset;
        private List<Tile> tiles;

        private Texture spriteSheet;

        private int height;
        private int width;

        public TileMap(Texture spriteSheet, int[,] map, int tileSize = 64)
        {
            this.tiles = new List<Tile>();
            this.spriteSheet = spriteSheet;
            this.map = map;
            this.tileSize = tileSize;
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

            width = w;
            height = h;

            //tiles.Add(new Tile(0));
            tiles = new List<Tile>(width * height);
            for (int i = 0; i < width * height; i++)
            {
                tiles.Add(new Tile(0));
            }
            //Console.WriteLine($"Width: {w}, Height: {h}");

            Refresh();
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

            var index = vx + vy * width;
            var rec = new FloatRect(x * tileSize, y * tileSize, tileSize, tileSize);


            //Console.WriteLine($"vx {vx} {vy} {width}");
            var textureX = (map[vy, vx] * 8) % width;
            var textureY = (map[vy, vx] * 8) % height;
            IntRect src = new IntRect(textureX, textureY, 8, 8);

            tiles[index].UpdateTile(rec, src);

        }


        /// <summary>
        /// Update offset (based on Target's position) and draw it
        /// </summary>
        public void Draw(RenderTarget rt, RenderStates states)
        {
            var view = rt.GetView();
            states.Texture = spriteSheet;
            Console.WriteLine(spriteSheet.Size.X);

            SetSize(view.Size);
            SetCorner(rt.MapPixelToCoords(new Vector2i()));

            foreach (Tile tile in tiles)
            {
                rt.Draw(tile.vertices, PrimitiveType.Quads, states);
            }
            
        }
    }
}
