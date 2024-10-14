using SnakeGame.Models;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Services;

namespace SnakeGame.Strategies
{
    public class FastStrategy : IStrategy
    {
        public int GetMoveCounter()
        {
            return 3;
        }

        public bool IsCollision(Point point)
        {
            // Check collision with walls
            if (GameService.Instance.Map.Grid[point.X, point.Y] == Map.CellType.Wall)
                return true;

            // Check collision with self
            if (GameService.Instance.Map.Grid[point.X, point.Y] == Map.CellType.Snake)
                return true;

            return false;
        }

        public bool DirectionReset()
        {
            return false;
        }
    }
}
