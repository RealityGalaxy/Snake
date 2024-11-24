using SnakeGame.Iterators;

namespace SnakeGame.Composites
{
    public class MovableComposite : IMovable
    {
        private List<IMovable> _movables = new List<IMovable>();

        public void Add(IMovable movable)
        {
            _movables.Add(movable);
        }

        public void Remove(IMovable movable)
        {
            _movables.Remove(movable);
        }

        public void Move()
        {
            foreach (var movable in _movables)
            {
                movable.Move();
            }
        }

        public void GenerateNewPosition()
        {
            foreach (var movable in _movables)
            {
                movable.GenerateNewPosition();
            }
        }

        // If this gets uncommented, it gives external users direct access to Composite components, therefore moving from reduced visibility to a full visibility component
        //
        //public IEnumerable<IMovable> GetMovables()
        //{
        //    return _movables;
        //}
    }
}
