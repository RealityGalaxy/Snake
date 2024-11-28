using SnakeGame.Handlers;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models;
using SnakeGame.ResponsibilityChains;
using SnakeGame.Services;

public class SelfCollisionHandler : ICollisionHandler
{
    private readonly ICollisionHandler _next;

    public SelfCollisionHandler(ICollisionHandler next)
    {
        _next = next;
    }

    public CollisionResult HandleCollision(Point newHead, Snake snake)
    {
        if (GameService.Instance.GameInstances[GameService.Instance.GetInstance(snake.ConnectionId)].Map.Grid[newHead.X, newHead.Y] == Map.CellType.Snake)
        {
            return new CollisionResult { KillSnake = true }; // Snake is dead
        }

        return _next?.HandleCollision(newHead, snake) ?? new CollisionResult();
    }
}
