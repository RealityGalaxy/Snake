using SnakeGame.Models;

namespace SnakeGame.Strategies.Decorators
{
    public class GhostDecorator : IStrategy
    {
        private readonly IStrategy _baseStrategy;
        public GhostDecorator(IStrategy strategy)
        {
            _baseStrategy = strategy;
        }
        public bool DirectionReset()
        {
            return _baseStrategy.DirectionReset();
        }

        public int GetMoveCounter()
        {
            return _baseStrategy.GetMoveCounter();
        }

        public bool IsCollision(Point point, int instance)
        {
            return false;
        }

        public IStrategy BaseStrategy()
        { 
            return _baseStrategy; 
        }
    }
}
