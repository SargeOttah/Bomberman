using BombermanServer.Services.Iterator;

namespace BombermanServer.Services.Strategies
{
    public interface IPlayerEmptyIdStrategy
    {
        int GetEmptyId(IIterator playerIterator);
    }
}
