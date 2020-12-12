using System;
using System.Collections.Generic;
using System.Text;
using Bomberman.Map;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Bomberman.Spawnables.Tiles
{
    public class Ground : ITile
    {
        private readonly Vertex[] _vertices;
        public int TileIndex { get; private set; }
        private RectangleShape DebugShape { get; set; }


        private Ground()
        {
            _vertices = new Vertex[4];
        }

        public Ground(int tileIndex) : this()
        {
            this.TileIndex = tileIndex;
        }

        public void InitDebug()
        {
            Vector2f tempDim = new Vector2f(GetBounds().X, GetBounds().Y);
            DebugShape = new RectangleShape(tempDim);
            DebugShape.FillColor = SFML.Graphics.Color.Transparent;
            DebugShape.OutlineColor = SFML.Graphics.Color.Red;
            DebugShape.OutlineThickness = 1f;
            DebugShape.Position = new Vector2f(_vertices[0].Position.X, _vertices[0].Position.Y);
        }

        public Vector2f GetBounds()
        {
            float width = _vertices[1].Position.X - _vertices[0].Position.X;
            float heigth = _vertices[3].Position.Y - _vertices[0].Position.Y;
            return new Vector2f(width, heigth);
        }

        public Vertex[] GetVertices()
        {
            return _vertices;
        }

        /// <summary>
        /// Initialises the vertex array
        /// </summary>
        /// <param name="rec">Tile location in the map</param>
        /// <param name="src">Sprite location in sprite sheet</param>
        public void UpdateTile(FloatRect rec, IntRect src)
        {
            _vertices[0].Position.X = rec.Left;
            _vertices[0].Position.Y = rec.Top;
            _vertices[0].TexCoords.X = src.Left;
            _vertices[0].TexCoords.Y = src.Top;
            _vertices[0].Color = Color.White;

            _vertices[1].Position.X = rec.Left + rec.Width;
            _vertices[1].Position.Y = rec.Top;
            _vertices[1].TexCoords.X = src.Left + src.Width;
            _vertices[1].TexCoords.Y = src.Top;
            _vertices[1].Color = Color.White;

            _vertices[2].Position.X = rec.Left + rec.Width;
            _vertices[2].Position.Y = rec.Top + rec.Height;
            _vertices[2].TexCoords.X = src.Left + src.Width;
            _vertices[2].TexCoords.Y = src.Top + src.Height;
            _vertices[2].Color = Color.White;

            _vertices[3].Position.X = rec.Left;
            _vertices[3].Position.Y = rec.Top + rec.Height;
            _vertices[3].TexCoords.X = src.Left;
            _vertices[3].TexCoords.Y = src.Top + src.Height;
            _vertices[3].Color = Color.White;

            InitDebug();

            //Console.WriteLine(vertices[0].Position.X);
            //Console.WriteLine($"{vertices[0].TexCoords.X} {vertices[0].TexCoords.Y} {vertices[1].TexCoords.X} {vertices[1].TexCoords.Y} {vertices[2].TexCoords.X} {vertices[2].TexCoords.Y} {vertices[3].TexCoords.X} {vertices[3].TexCoords.Y}");
        }
    }
}
