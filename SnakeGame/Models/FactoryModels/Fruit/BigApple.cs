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

            for(int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    Point one = new Point(x+i, y+j);
                    Instance.Map.Grid[x+i, y+j] = Map.CellType.Consumable;
                    Strawberry strawberry = new Strawberry(Instance);
                    strawberry.Position = one;
                    Instance.Consumables.Add(one, strawberry);
                }
            }
        }
        public override bool CanConsume()
        {
            throw new NotImplementedException();
        }

        public override int Consume()
        {
            throw new NotImplementedException();
        }
    }
}
