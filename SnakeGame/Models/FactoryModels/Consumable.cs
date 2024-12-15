using SnakeGame.Models.FactoryModels.Fruit.Attributes;
using SnakeGame.Services;
using SnakeGame.Composites;
using SnakeGame.Visitor;

namespace SnakeGame.Models.FactoryModels
{
    public abstract class Consumable : IPrototype<Consumable>, IMovable
    {
        public GameInstance Instance { get; set; }
        public Point Position { get; set; }
        public bool IsPoisonous { get; set; }
        public bool IsDynamic { get; set; }
        public bool IsBigConsumable { get; set; }
        public FruitAttributes Attributes { get; set; }

        public void Place(Point position, GameInstance instance)
        {
            Position = position;
            Instance = instance;
            Instance.Map.Grid[Position.X, Position.Y] = Map.CellType.Consumable;
        }

        public abstract bool CanConsume();
        public abstract int Consume();
        public abstract Consumable Clone();

        public virtual void GenerateNewPosition()
        {
            var random = new Random();
            int x, y;

            do
            {
                x = random.Next(1, Instance.Map.Size.Width - 1);
                y = random.Next(1, Instance.Map.Size.Height - 1);
            } while (Instance.Map.Grid[x, y] != Map.CellType.Empty);

            Position = new Point(x, y);
            Instance.Map.Grid[x, y] = Map.CellType.Consumable;
        }

        public void Remove()
        {
            Instance.Map.Grid[Position.X, Position.Y] = Map.CellType.Empty;
        }

        public void Move()
        {
            if (Random.Shared.Next(0, 100) < 50)
            {
                return;
            }

            if (!IsDynamic)
            {
                return;
            }

            Random random = new Random();
            int direction = random.Next(0, 4);
            Point newPosition = Position;

            switch (direction)
            {
                case 0:
                    newPosition = new Point(Position.X + 1, Position.Y);
                    break;
                case 1:
                    newPosition = new Point(Position.X - 1, Position.Y);
                    break;
                case 2:
                    newPosition = new Point(Position.X, Position.Y + 1);
                    break;
                case 3:
                    newPosition = new Point(Position.X, Position.Y - 1);
                    break;
            }

            if (Instance.Map.Grid[newPosition.X, newPosition.Y] == Map.CellType.Empty)
            {
                Instance.Map.Grid[Position.X, Position.Y] = Map.CellType.Empty;
                Position = newPosition;
                Instance.Map.Grid[Position.X, Position.Y] = Map.CellType.Consumable;
            }
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
