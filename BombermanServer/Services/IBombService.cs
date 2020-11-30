using BombermanServer.Models;
using System;
using System.Collections.Generic;

namespace BombermanServer.Services
{
    public interface IBombService
    {
        public void Add(Bomb bomb);
        public List<Bomb> GetBombs();
    }
}