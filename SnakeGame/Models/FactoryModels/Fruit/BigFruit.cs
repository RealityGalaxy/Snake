using SnakeGame.Services;
using SnakeGame.Models.FactoryModels.Fruit.Attributes;

namespace SnakeGame.Models.FactoryModels.Fruit
{
    public class BigFruit : Consumable
    {
        public BigFruit(GameInstance instance, FruitAttributes attributes)
        {
            Attributes = attributes;
            Instance = instance;
            GenerateNewPosition();
        }
        public override void GenerateNewPosition()
        {
            var random = new Random();
            int x, y;

            x = random.Next(1, Instance.Map.Size.Width - 1);
            y = random.Next(1, Instance.Map.Size.Height - 1);
            while (Instance.Map.Grid[x, y] != Map.CellType.Empty ||
            Instance.Map.Grid[x + 1, y + 1] != Map.CellType.Empty ||
            Instance.Map.Grid[x + 1, y] != Map.CellType.Empty ||
            Instance.Map.Grid[x, y + 1] != Map.CellType.Empty)
            {
                x = random.Next(1, Instance.Map.Size.Width - 1);
                y = random.Next(1, Instance.Map.Size.Height - 1);
            }

            for(int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    Point one = new Point(x+i, y+j);
                    Instance.Map.Grid[x+i, y+j] = Map.CellType.Consumable;
                    Fruit fruit = new Fruit(Instance, Attributes);
                    fruit.Position = one;
                    Instance.Consumables.Add(one, fruit);
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
