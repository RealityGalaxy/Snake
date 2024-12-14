using SnakeGame.Handlers;
using SnakeGame.Models;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.ResponsibilityChains;
using SnakeGame.Services;
using static SnakeGame.Models.Snake;

namespace SnakeGame.Template
{
    public abstract class MovementTemplate
    {
        private readonly ICollisionHandler _collisionHandler;

        public MovementTemplate()
        {
            //Chain of responsibility set-up
            _collisionHandler = new WallCollisionHandler(
            new SelfCollisionHandler(
                new FruitCollisionHandler(
                    new PoisonCollisionHandler(null)))); // End of chain
        }

        public void Move(Snake snake)
        {
            if (!snake.IsAlive) return;

            if (snake.SpawnTimer != 0)
            {
                snake.SpawnTimer--;
                return;
            }

            if (snake.MoveTimer != 0)
            {
                snake.MoveTimer = Math.Min(snake.MoveTimer - 1, GetMoveCounter());
                return;
            }

            snake.MoveTimer = GetMoveCounter();

            if (snake.RainbowTimer <= 0)
            {
                snake.Movement = snake.BaseMovement;
                snake.StopRainbowing();
            }
            else
            {
                snake.RainbowTimer--;
            }

            var head = snake.Body.First.Value;
            Point newHead = GetNextHeadPosition(head, snake);

            // Implementing chain of responsibility here

            CollisionResult collisionResult = _collisionHandler.HandleCollision(newHead, snake);

            if (collisionResult.KillSnake && snake.CurrentDirection != Direction.None)
            {
                snake.IsAlive = false;
                return;
            }
            // ------------------------------------------

            // Grow the snake by not removing the tail
            if (snake.tempFood > 0 && snake.CurrentDirection != Direction.None)
            {
                snake.tempFood--;
            }
            else if (snake.CurrentDirection != Direction.None)
            {
                // Remove the tail (move forward)
                var tail = snake.Body.Last.Value;
                GameService.Instance.GameInstances[GameService.Instance.GetInstance(snake.ConnectionId)].Map.Grid[tail.X, tail.Y] = GameService.Instance.GameInstances[GameService.Instance.GetInstance(snake.ConnectionId)].Map.Grid[newHead.X, newHead.Y] == Map.CellType.Wall
                ? Map.CellType.Wall
                : Map.CellType.Empty;
                snake.Body.RemoveLast();
                if (snake.tempFood < 0)
                {
                    //remove body for each point of poison
                    while (snake.tempFood < 0)
                    {
                        if (snake.Body.Count == 0)
                        {
                            snake.IsAlive = false;
                            return;
                        }
                        tail = snake.Body.Last.Value;
                        GameService.Instance.GameInstances[GameService.Instance.GetInstance(snake.ConnectionId)].Map.Grid[tail.X, tail.Y] = GameService.Instance.GameInstances[GameService.Instance.GetInstance(snake.ConnectionId)].Map.Grid[newHead.X, newHead.Y] == Map.CellType.Wall
                        ? Map.CellType.Wall
                        : Map.CellType.Empty;
                        snake.Body.RemoveLast();
                        snake.tempFood++;
                    }
                }
            }

            if (snake.CurrentDirection != Direction.None)
            {
                // Add new head
                snake.Body.AddFirst(newHead);
                GameService.Instance.GameInstances[GameService.Instance.GetInstance(snake.ConnectionId)].Map.Grid[newHead.X, newHead.Y] = GameService.Instance.GameInstances[GameService.Instance.GetInstance(snake.ConnectionId)].Map.Grid[newHead.X, newHead.Y] == Map.CellType.Wall
                    ? Map.CellType.Wall
                    : Map.CellType.Snake;
            }

            if (DirectionReset())
            {
                snake.CurrentDirection = Direction.None;
            }
        }

        protected virtual int GetMoveCounter()
        {
            return 6;
        }

        protected virtual bool IsCollision(Point point, int instance)
        {
            // Check collision with walls
            if (GameService.Instance.GameInstances[instance].Map.Grid[point.X, point.Y] == Map.CellType.Wall)
                return true;

            // Check collision with self
            if (GameService.Instance.GameInstances[instance].Map.Grid[point.X, point.Y] == Map.CellType.Snake)
                return true;

            return false;
        }

        protected virtual bool DirectionReset()
        {
            return false;
        }

        private Point GetNextHeadPosition(Point head, Snake snake)
        {
            return snake.CurrentDirection switch
            {
                Direction.Up => new Point(head.X, head.Y - 1),
                Direction.Down => new Point(head.X, head.Y + 1),
                Direction.Left => new Point(head.X - 1, head.Y),
                Direction.Right => new Point(head.X + 1, head.Y),
                _ => head,
            };
        }

        private bool IsFood(Point head, Snake snake)
        {
            if (GameService.Instance.GameInstances[GameService.Instance.GetInstance(snake.ConnectionId)].Map.Grid[head.X, head.Y] == Map.CellType.Consumable)
                return true;

            return false;
        }
    }
}
