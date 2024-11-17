using SnakeGame.Models;

namespace SnakeGame.Iterators
{
    public class PointIterator : IIterator<Point>
    {
        private readonly LinkedList<Point> _points;
        private int _index = 0;

        public PointIterator(LinkedList<Point> points)
        {
            _points = points;
        }

        public bool HasNext()
        {
            return _index < _points.Count;
        }

        public Point Next()
        {
            if (!HasNext())
            {
                throw new InvalidOperationException("No more elements to iterate over.");
            }
            return _points.ElementAt(_index++);
        }
    }
}
