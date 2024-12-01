using SnakeGame.Handlers;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models;
using SnakeGame.ResponsibilityChains;
using SnakeGame.Services;
using SnakeGame.Template;

public class FruitCollisionHandler : ICollisionHandler
{
    private readonly ICollisionHandler _next;

    public FruitCollisionHandler(ICollisionHandler next)
    {
        _next = next;
    }

    public CollisionResult HandleCollision(Point newHead, Snake snake)
    {
        var gameInstance = GameService.Instance.GameInstances[GameService.Instance.GetInstance(snake.ConnectionId)];
        if (gameInstance.Map.Grid[newHead.X, newHead.Y] == Map.CellType.Consumable)
        {
            var food = gameInstance.Consumables[newHead];
            if (food != null)
            {
                snake.tempFood += food.Consume();
                if (food is RainbowFruit && snake.RainbowTimer <= 0)
                {
                    snake.Movement = new FastMovementTemplate();
                    snake.StartRainbowing();
                    snake.RainbowTimer = 20;
                }

                gameInstance.Consumables.Remove(newHead);
            }
            return new CollisionResult { GrowSnake = true }; // Snake eats and grows
        }

        return _next?.HandleCollision(newHead, snake) ?? new CollisionResult();
    }
}
