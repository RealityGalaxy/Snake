using SnakeGame.Models;

namespace SnakeGame.Strategies.Decorators
{
    public class SlowDecorator : IStrategy
    {
        private readonly IStrategy _baseStrategy;
        public SlowDecorator(IStrategy strategy) 
        {
            _baseStrategy = strategy;
        }
        public bool DirectionReset()
        {
            return _baseStrategy.DirectionReset();
        }

        public int GetMoveCounter()
        {
            return _baseStrategy.GetMoveCounter() + 2;
        }

        public bool IsCollision(Point point, int instance)
        {
            return _baseStrategy.IsCollision(point, instance);
        }
        public IStrategy BaseStrategy()
        {
            return _baseStrategy;
        }
    }
}
