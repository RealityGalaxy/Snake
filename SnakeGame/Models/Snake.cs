using System;
using System.Collections.Generic;
using SnakeGame.Models.Consumables;
using SnakeGame.Services;

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
            if (Math.Abs((int)direction - (int)CurrentDirection) % 3 <= 1)
            {
                CurrentDirection = direction;
            }
        }

        private int tempFood = 0;
        public void Move()
        {
            if (!IsAlive) return;

            var head = Body.First.Value;
            Point newHead = GetNextHeadPosition(head);

            // Collision detection with walls or self
            if (IsCollision(newHead))
            {
                IsAlive = false;
                return;
            }
            // Check for fruit consumption
            if (IsFood(newHead))
            {
                // Eat the fruit
                Consumable food = _gameService.Consumables[newHead];
                if (food != null)
                {
                    tempFood += food.Consume();
                }
                _gameService.Consumables.Remove(newHead);
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
                Map.Instance.Grid[tail.X, tail.Y] = Map.CellType.Empty;
                Body.RemoveLast();
            }

            // Add new head
            Body.AddFirst(newHead);
            Map.Instance.Grid[newHead.X, newHead.Y] = Map.CellType.Snake;
        }

        private bool IsFood(Point head)
        {
            if (Map.Instance.Grid[head.X, head.Y] == Map.CellType.Consumable)
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

        private bool IsCollision(Point point)
        {
            // Check collision with walls
            if (Map.Instance.Grid[point.X, point.Y] == Map.CellType.Wall)
                return true;

            // Check collision with self
            if (Map.Instance.Grid[point.X, point.Y] == Map.CellType.Snake)
                return true;

            return false;
        }

        public enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }
    }
}
