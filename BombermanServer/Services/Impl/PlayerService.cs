using BombermanServer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BombermanServer.Services.Impl
{
    public class PlayerService : IPlayerService
    {
        List<Player> players;

        public PlayerService()
        {
            players = new List<Player>(4);
        }

        public bool AddPlayer(Player player)
        {
            if (players.Count >= 4) { return false; }

            players.Add(player);
            return true;
        }

        public Player GetPlayer(string connectionId)
        {
            return players.Find(x => x.connectionId.Equals(connectionId));
        }

        public int GetCount()
        {
            return players.Count;
        }

        public List<Player> GetPlayers()
        {
            return players;
        }

        public bool RemovePlayer(Player player)
        {
            return players.Remove(player);
        }
    }
}
