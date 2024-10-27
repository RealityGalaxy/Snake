using SnakeGame.Services;

namespace SnakeGame.Models.FactoryModels.Fruit
{
    public class BigApple : Consumable
    {
        public BigApple(GameInstance instance)
        {
            Instance = instance;
            GenerateNewPosition();
        }
        public override void GenerateNewPosition()
        {
            var random = new Random();
            int x, y;

            x = random.Next(1, Instance.Map.Width - 1);
            y = random.Next(1, Instance.Map.Height - 1);
            while (Instance.Map.Grid[x, y] != Map.CellType.Empty ||
            Instance.Map.Grid[x + 1, y + 1] != Map.CellType.Empty ||
            Instance.Map.Grid[x + 1, y] != Map.CellType.Empty ||
            Instance.Map.Grid[x, y + 1] != Map.CellType.Empty)
            {
                x = random.Next(1, Instance.Map.Width - 1);
                y = random.Next(1, Instance.Map.Height - 1);
            }

            this.Position = new Point(x, y);
        }
        public override bool CanConsume()
        {
            throw new NotImplementedException();
        }

        public override int Consume()
        {
            throw new NotImplementedException();
        }

        public override Consumable Clone()
        {
            Consumable clone = new Strawberry(Instance, Position)
            {
                IsPoisonous = this.IsPoisonous,
                IsDynamic = this.IsDynamic,
            };
            return clone;
        }
    }
}
