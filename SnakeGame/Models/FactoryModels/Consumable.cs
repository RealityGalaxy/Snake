using SnakeGame.Services;
using System;

namespace SnakeGame.Models.FactoryModels
{
    public abstract class Consumable
    {
        public GameInstance Instance { get; set; }
        public Point Position { get; set; }
        public string Color { get; set; }
        public int Value { get; set; }
        public bool IsPoisonous { get; set; } 

        public void Place(Point position, GameInstance instance)
        {
            Position = position;
            Instance = instance;
            Instance.Map.Grid[Position.X, Position.Y] = Map.CellType.Consumable;
        }

        public abstract bool CanConsume();
        public abstract int Consume();

        public virtual void GenerateNewPosition()
        {
            var random = new Random();
            int x, y;

            do
            {
                x = random.Next(1, Instance.Map.Width - 1);
                y = random.Next(1, Instance.Map.Height - 1);
            } while (Instance.Map.Grid[x, y] != Map.CellType.Empty);

            Position = new Point(x, y);
            Instance.Map.Grid[x, y] = Map.CellType.Consumable;
        }

        public void Remove()
        {
            Instance.Map.Grid[Position.X, Position.Y] = Map.CellType.Empty;
        }
    }
}
