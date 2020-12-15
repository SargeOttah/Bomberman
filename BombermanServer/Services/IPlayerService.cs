using BombermanServer.Models;
using BombermanServer.Services.Iterator;

namespace BombermanServer.Services
{
    public interface IPlayerService
    {
        public IIterator GetPlayerIterator();

        public PlayerDTO GetPlayer(string connectionId);

        public int GetCount();

        public bool AddPlayer(PlayerDTO player);

        public bool RemovePlayer(PlayerDTO player);

        public int GetEmptyId();
        public void KillPlayer(int id);
    }
}
