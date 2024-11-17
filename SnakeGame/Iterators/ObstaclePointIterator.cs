using SnakeGame.Models;

namespace SnakeGame.Iterators
{
    public class ObstaclePointIterator : IIterator<Point>
    {
        private readonly int[,] _points;
        private int _rows = 0;
        private int _cols = 0;
        private int _currentRow = 0;
        private int _currentCol = 0;

        public ObstaclePointIterator(int[,] points)
        {
            _points = points;
            _rows = _points.GetLength(0);
            _cols = _points.GetLength(1);
        }

        public bool HasNext()
        {
            while (_currentRow < _rows)
            {
                // Skip cells where value is not 1
                if (_points[_currentRow, _currentCol] == 1)
                {
                    return true;
                }

                MoveToNext();
            }
            return false;
        }

        public Point Next()
        {
            if (!HasNext())
            {
                throw new InvalidOperationException("No more elements in the iterator.");
            }

            // Return the current valid point
            Point result = new Point(_currentCol, _currentRow);

            // Move to the next cell
            MoveToNext();

            return result;
        }

        private void MoveToNext()
        {
            _currentCol++;
            if (_currentCol >= _cols)
            {
                _currentCol = 0;
                _currentRow++;
            }
        }
    }
}
