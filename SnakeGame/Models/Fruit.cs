using System;
using SnakeGame.Models;

namespace SnakeGame.Models
{
    public class Fruit
    {
        public Point Position { get; private set; }

        public Fruit()
        {
            GenerateNewPosition();
        }

        public void GenerateNewPosition()
        {
            var random = new Random();
            int x, y;

            do
            {
                x = random.Next(1, Map.Instance.Width - 1);
                y = random.Next(1, Map.Instance.Height - 1);
            } while (Map.Instance.Grid[x, y] != Map.CellType.Empty);

            Position = new Point(x, y);
            Map.Instance.Grid[x, y] = Map.CellType.Fruit;
        }

        public void Remove()
        {
            Map.Instance.Grid[Position.X, Position.Y] = Map.CellType.Empty;
        }
    }
}
