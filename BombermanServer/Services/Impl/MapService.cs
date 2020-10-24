using BombermanServer.Models;
using System.Collections.Generic;

namespace BombermanServer.Services.Impl
{
    public class MapService : IMapService
    {
        List<Player> players;
        string[] map;

        public void LoadMap(int id) // load from file or whatever
        {
            map = new string[11]
            {
                "2,S,2,2,2,2,2,2,2,2,2,2,2,2,2",
                "2,2,2,2,2,2,2,2,2,2,2,2,2,2,2",
                "O,2,2,2,2,2,2,2,2,2,2,2,2,2,2",
                "2,2,2,2,2,2,2,2,2,2,2,2,2,2,2",
                "B,2,2,2,2,2,2,2,2,2,2,2,2,2,2",
                "2,2,2,2,2,2,2,2,2,2,2,2,2,2,2",
                "C,2,2,2,2,2,2,2,2,2,2,2,2,2,2",
                "2,2,2,2,2,2,2,2,2,2,2,2,2,2,2",
                "2,2,2,2,2,2,2,2,2,2,2,2,2,2,2",
                "2,2,2,2,2,2,2,2,2,2,2,2,2,2,2",
                "2,2,2,2,2,2,2,2,2,2,2,2,2,2,2"
            };
        }

        public string[] GetMap()
        {
            return map;
        }
    }
}
