using BombermanServer.Models;
using System;
using System.Collections.Generic;

namespace BombermanServer.Services
{
    public interface IBombService
    {
        public void Add(BombDTO bomb);
        public List<BombDTO> GetBombs();
    }
}