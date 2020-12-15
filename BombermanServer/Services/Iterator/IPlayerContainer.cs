using BombermanServer.Models;

namespace BombermanServer.Services.Iterator
{
    public interface IPlayerContainer
    {
        IIterator GetIterator();
        void AddPlayer(Player player);
        int GetCount();
        bool RemovePlayer(Player player);
        void KillPlayer(int id);
    }
}
