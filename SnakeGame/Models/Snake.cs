using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;
using System.Threading;
using System.Xml.Linq;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.Services;
using SnakeGame.Strategies;
using SnakeGame.Strategies.Decorators;

namespace SnakeGame.Models
{
    public class Snake
    {
        public string ConnectionId { get; }
        public LinkedList<Point> Body { get; }
        public Direction CurrentDirection { get; set; }
        private string BaseColor { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public bool IsAlive { get; set; } = true;
        public int MoveTimer { get; set; } = 6;
        private IStrategy BaseStrategy { get; set; }
        public IStrategy CurrentStrategy { get; set; }
        private GameService _gameService;

        public Snake(string connectionId, Point startPosition, GameService gameService, string color, string name, IStrategy strategy)
        {
            ConnectionId = connectionId;
            Body = new LinkedList<Point>();
            Body.AddFirst(startPosition);
            CurrentDirection = Direction.Right;
            _gameService = gameService;
            BaseColor = color;
            Color = BaseColor;
            Name = name;
            BaseStrategy = strategy;
            CurrentStrategy = BaseStrategy;
        }

        public void Turn(Direction direction)
        {
            if (!IsAlive) return;
            if(CurrentDirection == Direction.None)
            {
                CurrentDirection = direction;
                return;
            }
            if (Math.Abs((int)direction - (int)CurrentDirection) % 3 <= 1)
            {
                CurrentDirection = direction;
            }
        }

        private int tempFood = 0;
        private int RainbowTimer = 20;
        private int SpawnTimer = 24;
        public void Move()
        {
            if(Body.Count < 5 && CurrentStrategy is not SmallDecorator)
            {
                CurrentStrategy = new SmallDecorator(CurrentStrategy);
            }
            else if (Body.Count >= 5 && CurrentStrategy is SmallDecorator)
            {
                CurrentStrategy = CurrentStrategy.BaseStrategy();
            }


            if (Body.Count >= 15 && CurrentStrategy is not SlowDecorator)
            {
                CurrentStrategy = new SlowDecorator(CurrentStrategy);
            }
            else if (Body.Count < 15 && CurrentStrategy is SlowDecorator)
            {
                CurrentStrategy = CurrentStrategy.BaseStrategy();
            }

            if (!IsAlive) return;
            if (SpawnTimer != 0)
            {
                SpawnTimer--;
                return;
            }
            if (MoveTimer != 0)
            {
                MoveTimer = Math.Min(MoveTimer-1, CurrentStrategy.GetMoveCounter());
                return;
            }
            MoveTimer = CurrentStrategy.GetMoveCounter();
            if (RainbowTimer <= 0 && (CurrentStrategy is FastStrategy || CurrentStrategy.BaseStrategy() is FastStrategy))
            {
                CurrentStrategy = BaseStrategy;
                StopRainbowing();
            }
            else
            {
                RainbowTimer--;
            }

            var head = Body.First.Value;
            Point newHead = GetNextHeadPosition(head);

            // Collision detection with walls or self
            if (CurrentStrategy.IsCollision(newHead, _gameService.GetInstance(ConnectionId)) && CurrentDirection != Direction.None)
            {
                IsAlive = false;
                return;
            }
            // Check for fruit consumption
            if (IsFood(newHead))
            {
                // Eat the fruit
                Consumable food = _gameService.GameInstances[_gameService.GetInstance(ConnectionId)].Consumables[newHead];
                if (food != null)
                {
                    tempFood += food.Consume();
                }
                if(food is RainbowFruit)
                {
                    CurrentStrategy = new FastStrategy();
                    StartRainbowing();
                    RainbowTimer = 20;
                }
                _gameService.GameInstances[_gameService.GetInstance(ConnectionId)].Consumables.Remove(newHead);
            }

            // Grow the snake by not removing the tail
            if (tempFood > 0 && CurrentDirection != Direction.None)
            {
                tempFood--;
            }
            else if (CurrentDirection != Direction.None)
            {
                // Remove the tail (move forward)
                var tail = Body.Last.Value;
                GameService.Instance.GameInstances[_gameService.GetInstance(ConnectionId)].Map.Grid[tail.X, tail.Y] = GameService.Instance.GameInstances[_gameService.GetInstance(ConnectionId)].Map.Grid[newHead.X, newHead.Y] == Map.CellType.Wall
                ? Map.CellType.Wall
                : Map.CellType.Empty;
                Body.RemoveLast();
                if (tempFood < 0)
                {
                    //remove body for each point of poison
                    while (tempFood < 0)
                    {
                        if (Body.Count == 0)
                        {
                            IsAlive = false;
                            return;
                        }
                        tail = Body.Last.Value;
                        GameService.Instance.GameInstances[_gameService.GetInstance(ConnectionId)].Map.Grid[tail.X, tail.Y] = GameService.Instance.GameInstances[_gameService.GetInstance(ConnectionId)].Map.Grid[newHead.X, newHead.Y] == Map.CellType.Wall
                        ? Map.CellType.Wall
                        : Map.CellType.Empty;
                        Body.RemoveLast();
                        tempFood++;
                    }
                }
            }
            if(CurrentDirection != Direction.None)
            {
                // Add new head
                Body.AddFirst(newHead);
                GameService.Instance.GameInstances[_gameService.GetInstance(ConnectionId)].Map.Grid[newHead.X, newHead.Y] = GameService.Instance.GameInstances[_gameService.GetInstance(ConnectionId)].Map.Grid[newHead.X, newHead.Y] == Map.CellType.Wall
                    ? Map.CellType.Wall
                    : Map.CellType.Snake;
            }

            if (CurrentStrategy.DirectionReset())
            {
                CurrentDirection = Direction.None;
            }
        }

        private bool IsFood(Point head)
        {
            if (GameService.Instance.GameInstances[_gameService.GetInstance(ConnectionId)].Map.Grid[head.X, head.Y] == Map.CellType.Consumable)
                return true;

            return false;
        }

        private Point GetNextHeadPosition(Point head)
        {
            return CurrentDirection switch
            {
                Direction.Up => new Point(head.X, head.Y - 1),
                Direction.Down => new Point(head.X, head.Y + 1),
                Direction.Left => new Point(head.X - 1, head.Y),
                Direction.Right => new Point(head.X + 1, head.Y),
                _ => head,
            };
        }

        private CancellationTokenSource _cancellationTokenSource;
        public async void StartRainbowing()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            // Define a list of colors for the rainbow effect
            string[] rainbowColors = new[] { "#FF0000", "#FF7F00", "#FFFF00", "#00FF00", "#0000FF", "#4B0082", "#9400D3" };
            int colorIndex = 0;

            try
            {
                // Run the color-changing loop while the fruit is alive
                while (true)
                {
                    // Change the fruit's color
                    Color = rainbowColors[colorIndex];

                    // Cycle through the colors
                    colorIndex = (colorIndex + 1) % rainbowColors.Length;

                    // Wait for a short delay before changing the color again (e.g., 300 ms)
                    await Task.Delay(50, token);
                }
            }
            catch (TaskCanceledException)
            {
                Color = BaseColor;
            }
        }

        // Method to stop the rainbow process (when the fruit is consumed or removed)
        public void StopRainbowing()
        {
            _cancellationTokenSource?.Cancel(); // Stop the async loop
        }

        public enum Direction
        {
            Up,
            Right,
            Down,
            Left,
            None
        }
    }
}
