using BombermanServer.Models;
using System;
using System.Collections.Generic;

namespace BombermanServer.Services
{
    public interface IPlayerService
    {
        public List<Player> GetPlayers();

        public Player GetPlayer(String connectionId);

        public int GetCount();

        public bool AddPlayer(Player player);

        public bool RemovePlayer(Player player);

        public int GetEmptyId();
        public void KillPlayer(int id);
    }
}
