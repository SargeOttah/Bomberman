using BombermanServer.Models;
using System.Collections.Generic;

namespace BombermanServer.Services
{
    public interface IPlayerService
    {
        public List<PlayerDTO> GetPlayers();

        public PlayerDTO GetPlayer(string connectionId);

        public int GetCount();

        public bool AddPlayer(PlayerDTO player);

        public bool RemovePlayer(PlayerDTO player);

        public int GetEmptyId();
    }
}
