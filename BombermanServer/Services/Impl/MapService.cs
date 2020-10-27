using BombermanServer.Models;
using System.Collections.Generic;

namespace BombermanServer.Services.Impl
{
    public class MapService : IMapService
    {
        string currentName => nameof(MapService);
        string[] map;

        public void LoadMap(int id) // load from file or whatever
        {
            map = new string[9]
            {
                "S,S,S,S,S,S,S,S,S,S,S,S,S",
                "S,2,2,2,2,2,2,2,2,2,2,2,S",
                "S,2,O,2,S,2,O,2,S,2,O,2,S",
                "S,2,2,2,2,2,2,2,2,2,2,2,S",
                "S,2,S,2,O,2,S,2,O,2,S,2,S",
                "S,2,2,2,2,2,2,2,2,2,2,2,S",
                "S,2,O,C,S,B,O,2,S,2,O,2,S",
                "S,2,2,2,2,2,2,2,2,2,2,2,S",
                "S,S,S,S,S,S,S,S,S,S,S,S,S"
            };
        }

        public string[] GetMap()
        {
            return map;
        }

        public string GetServiceName()
        {
            return currentName;
        }
    }
}
