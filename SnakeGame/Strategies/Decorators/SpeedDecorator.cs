using SnakeGame.Models;

namespace SnakeGame.Strategies.Decorators
{
    public class SmallDecorator : IStrategy
    {
        private readonly IStrategy _baseStrategy;
        public SmallDecorator(IStrategy strategy) 
        {
            _baseStrategy = strategy;
        }
        public bool DirectionReset()
        {
            return _baseStrategy.DirectionReset();
        }

        public int GetMoveCounter()
        {
            return _baseStrategy.GetMoveCounter() / 2;
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
