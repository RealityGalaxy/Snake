using SnakeGame.Handlers;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models;
using SnakeGame.ResponsibilityChains;
using SnakeGame.Services;

public class PoisonCollisionHandler : ICollisionHandler
{
    private readonly ICollisionHandler _next;

    public PoisonCollisionHandler(ICollisionHandler next)
    {
        _next = next;
    }

    public CollisionResult HandleCollision(Point newHead, Snake snake)
    {
        var gameInstance = GameService.Instance.GameInstances[GameService.Instance.GetInstance(snake.ConnectionId)];

        // Check if the new head position contains a consumable
        if (gameInstance.Map.Grid[newHead.X, newHead.Y] == Map.CellType.Consumable)
        {
            if (gameInstance.Consumables.TryGetValue(newHead, out Consumable food) && food.IsPoisonous)
            {
                // Handle poison effect
                snake.tempFood -= food.Consume(); // Deduct points (negative tempFood shrinks the snake)
                gameInstance.Consumables.Remove(newHead);

                return new CollisionResult { ShrinkSnake = Math.Abs(food.Consume()) }; // Signal the snake should shrink
            }
        }

        // Pass to the next handler in the chain
        return _next?.HandleCollision(newHead, snake) ?? new CollisionResult();
    }
}
