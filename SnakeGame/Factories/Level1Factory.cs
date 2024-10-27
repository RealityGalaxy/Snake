using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.Models.FactoryModels.Fruit.Attributes;
using SnakeGame.Models.FactoryModels.Maps.MapSizes;
using SnakeGame.Services;

namespace SnakeGame.Factories
{
    public class Level1Factory : ILevelFactory
    {
        public Obstacle generateObstacle()
        {
            return new SmallRock();
        }

        public Consumable generateConsumable(GameInstance instance)
        {
            Random foodRand = new Random();
            Consumable fruit;

            switch (foodRand.Next(0, 10))
            {
                case >= 9:
                    fruit = new Fruit(instance, new WatermelonAttributes());
                    break;
                case >= 6:
                    fruit = new Fruit(instance, new LemonAttributes());
                    break;
                default:
                    fruit = new Fruit(instance, new StrawberryAttributes());
                    break;
            }
            fruit.GenerateNewPosition();
            return fruit;
        }

        public Map generateMap(GameInstance instance)
        {
            return new Level1Map(instance, new MapSize20());
        }
    }
}
