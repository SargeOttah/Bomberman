using System;
using System.Collections.Generic;

namespace BombermanServer.Services
{
    public interface IMapService
    {
        public void LoadMap(int id = 0);
        public string[] GetMap();
        public string GetServiceName();
    }
}
