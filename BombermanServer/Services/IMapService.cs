using System;
using System.Collections.Generic;

namespace BombermanServer.Services
{
    public interface IMapService
    {
        public void LoadMap(int id);
        public string[] GetMap();
    }
}
