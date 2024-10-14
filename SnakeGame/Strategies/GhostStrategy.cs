using SnakeGame.Models;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Services;

namespace SnakeGame.Strategies
{
    public class GhostStrategy : IStrategy
    {
        public int GetMoveCounter()
        {
            return 4;
        }

        public bool IsCollision(Point point)
        {
            return false;
        }

        public bool DirectionReset()
        {
            return false;
        }
    }
}
