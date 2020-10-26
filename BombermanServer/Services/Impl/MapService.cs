using BombermanServer.Models;
using System.Collections.Generic;

namespace BombermanServer.Services.Impl
{
    public class MapService : IMapService
    {
        string currentName => nameof(MapService);
        List<Player> players;
        string[] map;

        public void LoadMap(int id) // load from file or whatever
        {
            map = new string[9]
            {
                "2,S,S,S,S,S,S,S,S,S,S,S,S",
                "2,2,2,2,2,2,2,2,2,2,2,2,S",
                "O,2,2,2,2,2,2,2,2,2,S,2,S",
                "2,2,2,2,2,2,2,2,2,2,2,2,S",
                "B,2,2,2,2,2,2,2,2,2,S,2,S",
                "2,2,2,2,2,2,2,2,2,2,2,2,S",
                "C,2,S,C,S,B,S,2,S,2,S,2,S",
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
