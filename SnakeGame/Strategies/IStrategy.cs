using SnakeGame.Models;

namespace SnakeGame.Strategies
{
    public interface IStrategy
    {
        public IStrategy BaseStrategy() { return null; }
        public int GetMoveCounter();

        public bool IsCollision(Point point, int instance);

        public bool DirectionReset();
    }
}
