namespace BombermanServer.Services.Iterator
{
    public interface IIterator
    {
        string GetNext();
        bool HasNext();
        void Reset();
    }
}
