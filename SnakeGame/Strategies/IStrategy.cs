using SnakeGame.Models;

namespace SnakeGame.Strategies
{
    public interface IStrategy
    {
        public int GetMoveCounter();

        public bool IsCollision(Point point, int instance);

        public bool DirectionReset();
    }
}
