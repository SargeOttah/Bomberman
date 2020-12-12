namespace BombermanServer.Services.Iterator
{
    public class DictionaryIterator : IIterator
    {
        private readonly string[,] _collection;

        private int _rowIndex;
        private int _columnIndex;

        public DictionaryIterator(string[,] collection)
        {
            _collection = collection;
        }

        public string GetNext()
        {
            if (!HasNext())
            {
                return string.Empty;
            }

            if (_columnIndex < _collection.GetLength(1))
            {
                _columnIndex++;

                return _collection[_rowIndex, _columnIndex - 1];
            }

            _rowIndex++;
            _columnIndex = 0;

            return _collection[_rowIndex, _columnIndex];

        }

        public bool HasNext() => (_rowIndex + 1) * (_columnIndex + 1) < _collection.Length + 1;

        public void Reset()
        {
            _rowIndex = 0;
            _columnIndex = 0;
        }
    }
}
