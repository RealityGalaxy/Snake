using System;

namespace SnakeGame.Models.Consumables
{
    public abstract class Consumable
    {
        public Point Position { get; set; }
        public string Color { get; set; }

        public void Place(Point position)
        {
            Position = position;
            Map.Instance.Grid[Position.X, Position.Y] = Map.CellType.Consumable;
        }

        public abstract bool CanConsume();
        public abstract int Consume();

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
            Map.Instance.Grid[x, y] = Map.CellType.Consumable;
        }

        public void Remove()
        {
            Map.Instance.Grid[Position.X, Position.Y] = Map.CellType.Empty;
        }
    }
}
