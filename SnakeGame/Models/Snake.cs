using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.Services;
using SnakeGame.Strategies;

namespace SnakeGame.Models
{
    public class Snake
    {
        public string ConnectionId { get; }
        public LinkedList<Point> Body { get; }
        public Direction CurrentDirection { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public bool IsAlive { get; set; } = true;
        public int MoveTimer { get; set; } = 6;
        public IStrategy CurrentStrategy { get; set; } = new BasicStrategy();
        private GameService _gameService;

        public Snake(string connectionId, Point startPosition, GameService gameService, string color, string name)
        {
            ConnectionId = connectionId;
            Body = new LinkedList<Point>();
            Body.AddFirst(startPosition);
            CurrentDirection = Direction.Right;
            _gameService = gameService;
            Color = color;
            Name = name;
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
            if (RainbowTimer == 0)
            {
                CurrentStrategy = new BasicStrategy();
            }
            else
            {
                RainbowTimer--;
            }

            var head = Body.First.Value;
            Point newHead = GetNextHeadPosition(head);

            // Collision detection with walls or self
            if (CurrentStrategy.IsCollision(newHead, _gameService.GetInstance(ConnectionId)))
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
                    RainbowTimer = 20;
                }
                _gameService.GameInstances[_gameService.GetInstance(ConnectionId)].Consumables.Remove(newHead);
            }

            // Grow the snake by not removing the tail
            if (tempFood > 0)
            {
                tempFood--;
            }
            else
            {
                // Remove the tail (move forward)
                var tail = Body.Last.Value;
                GameService.Instance.GameInstances[_gameService.GetInstance(ConnectionId)].Map.Grid[tail.X, tail.Y] = GameService.Instance.GameInstances[_gameService.GetInstance(ConnectionId)].Map.Grid[newHead.X, newHead.Y] == Map.CellType.Wall
                ? Map.CellType.Wall
                : Map.CellType.Empty;
                Body.RemoveLast();
            }

            // Add new head
            Body.AddFirst(newHead);
            GameService.Instance.GameInstances[_gameService.GetInstance(ConnectionId)].Map.Grid[newHead.X, newHead.Y] = GameService.Instance.GameInstances[_gameService.GetInstance(ConnectionId)].Map.Grid[newHead.X, newHead.Y] == Map.CellType.Wall
                ? Map.CellType.Wall
                : Map.CellType.Snake;

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
