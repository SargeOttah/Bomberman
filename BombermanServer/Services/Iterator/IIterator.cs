using BombermanServer.Models;

namespace BombermanServer.Services.Iterator
{
    public interface IIterator
    {
        Player GetNext();
        bool HasNext();
    }
}
