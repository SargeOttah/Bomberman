using BombermanServer.Models;

namespace BombermanServer.Services.Iterator
{
    public interface IPlayerContainer
    {
        IIterator GetIterator();
        void AddPlayer(PlayerDTO player);
        int GetCount();
        bool RemovePlayer(PlayerDTO player);
        void KillPlayer(int id);
    }
}
