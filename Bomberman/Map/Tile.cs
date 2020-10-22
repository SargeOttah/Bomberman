using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace Bomberman.Map
{
    public class Tile
    {
        public Vertex[] vertices { get; private set; }
        public int tileIndex { get; private set; }

        public Tile()
        {
            vertices = new Vertex[4];
        }

        public Tile(int tileIndex) : this()
        {
            this.tileIndex = tileIndex;
        }

        /// <summary>
        /// Redraws one tile
        /// </summary>
        /// <param name="rec">Tile location in the map</param>
        /// <param name="src">Sprite location in sprite sheet</param>
        public void UpdateTile(FloatRect rec, IntRect src)
        {
            
            vertices[0].Position.X = rec.Left;
            vertices[0].Position.Y = rec.Top;
            vertices[0].TexCoords.X = src.Left;
            vertices[0].TexCoords.Y = src.Top;
            vertices[0].Color = Color.White;

            vertices[1].Position.X = rec.Left + rec.Width;
            vertices[1].Position.Y = rec.Top;
            vertices[1].TexCoords.X = src.Left + src.Width;
            vertices[1].TexCoords.Y = src.Top;
            vertices[1].Color = Color.White;

            vertices[2].Position.X = rec.Left + rec.Width;
            vertices[2].Position.Y = rec.Top + rec.Height;
            vertices[2].TexCoords.X = src.Left + src.Width;
            vertices[2].TexCoords.Y = src.Top + src.Height;
            vertices[2].Color = Color.White;

            vertices[3].Position.X = rec.Left;
            vertices[3].Position.Y = rec.Top + rec.Height;
            vertices[3].TexCoords.X = src.Left;
            vertices[3].TexCoords.Y = src.Top + src.Height;
            vertices[3].Color = Color.White;

            //Console.WriteLine(vertices[0].Position.X);
            //Console.WriteLine($"{vertices[0].TexCoords.X} {vertices[0].TexCoords.Y} {vertices[1].TexCoords.X} {vertices[1].TexCoords.Y} {vertices[2].TexCoords.X} {vertices[2].TexCoords.Y} {vertices[3].TexCoords.X} {vertices[3].TexCoords.Y}");
        }
    }
}
