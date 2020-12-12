using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace Bomberman.Map
{
    internal class CompoundTile : ITile
    {
        private readonly List<ITile> _tiles = new List<ITile>();

        public void Add(ITile tile)
        {
            _tiles.Add(tile);
        }

        public bool Remove(ITile tile)
        {
            return _tiles.Remove(tile);
        }

        public Vertex[] GetVertices()
        {
            var vertices = new Vertex[_tiles.Count * 4];
            var index = 0;

            foreach (var tile in _tiles)
            {
                tile.GetVertices().CopyTo(vertices, index);
                index += 4;
            }

            return vertices;
        }
    }
}
