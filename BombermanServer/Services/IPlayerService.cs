using BombermanServer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BombermanServer.Services
{
    public interface IPlayerService
    {
        public List<Player> GetPlayers();

        public Player GetPlayer(String connectionId);

        public int GetCount();

        public bool AddPlayer(Player player);

        public bool RemovePlayer(Player player);
    }
}
