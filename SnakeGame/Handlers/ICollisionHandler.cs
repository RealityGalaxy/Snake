using SnakeGame.Handlers;
using SnakeGame.Models;

namespace SnakeGame.ResponsibilityChains
{
    public interface ICollisionHandler
    {
        CollisionResult HandleCollision(Point point, Snake snake);
    }

}
