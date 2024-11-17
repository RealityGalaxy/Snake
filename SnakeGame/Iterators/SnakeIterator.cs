using SnakeGame.Models;

namespace SnakeGame.Iterators
{
    public class SnakeIterator : IIterator<Snake>
    {
        private readonly Dictionary<string, Snake> _snakes;
        private int _index = 0;

        public SnakeIterator(Dictionary<string, Snake> snakes)
        {
            _snakes = snakes;
        }

        public bool HasNext()
        {
            return _index < _snakes.Count;
        }

        public Snake Next()
        {
            return HasNext() ? _snakes.ElementAt(_index++).Value : null;
        }
    }
}
