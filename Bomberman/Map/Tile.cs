using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Bomberman.Map
{
    public class Tile
    {
        public Vertex[] vertices { get; private set; }
        public int tileIndex { get; private set; }
        public RectangleShape debugShape { get; private set; }


        public Tile()
        {
            vertices = new Vertex[4];
        }

        public Tile(int tileIndex) : this()
        {
            this.tileIndex = tileIndex;
        }

        public void InitDebug()
        {
            Vector2f tempDim = new Vector2f(GetBounds().X, GetBounds().Y);
            debugShape = new RectangleShape(tempDim);
            debugShape.FillColor = SFML.Graphics.Color.Transparent;
            debugShape.OutlineColor = SFML.Graphics.Color.Red;
            debugShape.OutlineThickness = 1f;
            debugShape.Position = new Vector2f(vertices[0].Position.X, vertices[0].Position.Y);
        }

        public Vector2f GetBounds()
        {
            float width = vertices[1].Position.X - vertices[0].Position.X;
            float heigth = vertices[3].Position.Y - vertices[0].Position.Y;
            return new Vector2f(width, heigth);
        }

        /// <summary>
        /// Initialises the vertex array
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

            InitDebug();

            //Console.WriteLine(vertices[0].Position.X);
            //Console.WriteLine($"{vertices[0].TexCoords.X} {vertices[0].TexCoords.Y} {vertices[1].TexCoords.X} {vertices[1].TexCoords.Y} {vertices[2].TexCoords.X} {vertices[2].TexCoords.Y} {vertices[3].TexCoords.X} {vertices[3].TexCoords.Y}");
        }
    }
}
