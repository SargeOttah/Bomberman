namespace BombermanServer.Services.Iterator
{
    public class DictionaryContainer : IContainer
    {
        private readonly string[,] _map = 
        {
            { "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S" },
            { "S", "2", "2", "2", "2", "2", "2", "2", "2", "2", "2", "2", "S" },
            { "S", "2", "O", "2", "S", "2", "O", "2", "S", "2", "O", "2", "S" },
            { "S", "2", "2", "2", "2", "2", "2", "2", "2", "2", "2", "2", "S" },
            { "S", "2", "O", "2", "S", "2", "O", "2", "S", "2", "O", "2", "S" },
            { "S", "2", "2", "B", "2", "2", "C", "2", "2", "2", "2", "2", "S" },
            { "S", "2", "O", "C", "S", "B", "O", "2", "S", "2", "O", "2", "S" },
            { "S", "2", "C", "B", "2", "2", "2", "2", "2", "2", "2", "2", "S" },
            { "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S", "S" }
        };

        public IIterator GetIterator() => new DictionaryIterator(_map);
    }
}
