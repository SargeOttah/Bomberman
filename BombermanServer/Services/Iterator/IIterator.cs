using BombermanServer.Models;

namespace BombermanServer.Services.Iterator
{
    public interface IIterator
    {
        PlayerDTO GetNext();
        bool HasNext();
    }
}
