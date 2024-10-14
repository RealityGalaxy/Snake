using SnakeGame.Services;
using System;

namespace SnakeGame.Models.FactoryModels
{
    public abstract class Consumable
    {
        public Point Position { get; set; }
        public string Color { get; set; }
        public int Value { get; set; }

        public void Place(Point position)
        {
            Position = position;
            GameService.Instance.Map.Grid[Position.X, Position.Y] = Map.CellType.Consumable;
        }

        public abstract bool CanConsume();
        public abstract int Consume();

        public virtual void GenerateNewPosition()
        {
            var random = new Random();
            int x, y;

            do
            {
                x = random.Next(1, GameService.Instance.Map.Width - 1);
                y = random.Next(1, GameService.Instance.Map.Height - 1);
            } while (GameService.Instance.Map.Grid[x, y] != Map.CellType.Empty);

            Position = new Point(x, y);
            GameService.Instance.Map.Grid[x, y] = Map.CellType.Consumable;
        }

        public void Remove()
        {
            GameService.Instance.Map.Grid[Position.X, Position.Y] = Map.CellType.Empty;
        }
    }
}
