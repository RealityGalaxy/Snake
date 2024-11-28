using SnakeGame.Handlers;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models;
using SnakeGame.ResponsibilityChains;
using SnakeGame.Services;

public class WallCollisionHandler : ICollisionHandler
{
    private readonly ICollisionHandler _next;

    public WallCollisionHandler(ICollisionHandler next)
    {
        _next = next;
    }

    public CollisionResult HandleCollision(Point newHead, Snake snake)
    {
        if (GameService.Instance.GameInstances[GameService.Instance.GetInstance(snake.ConnectionId)].Map.Grid[newHead.X, newHead.Y] == Map.CellType.Wall)
        {
            return new CollisionResult { KillSnake = true }; // Snake is dead
        }

        return _next?.HandleCollision(newHead, snake) ?? new CollisionResult();
    }
}
