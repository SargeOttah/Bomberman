using BombermanServer.Models;
using BombermanServer.Services.Iterator;

namespace BombermanServer.Services
{
    public interface IPlayerService
    {
        public IIterator GetPlayerIterator();

        public Player GetPlayer(string connectionId);

        public int GetCount();

        public bool AddPlayer(Player player);

        public bool RemovePlayer(Player player);

        public int GetEmptyId();
        public void KillPlayer(int id);
    }
}
