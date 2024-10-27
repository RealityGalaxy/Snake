using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.Models.FactoryModels.Fruit.Attributes;
using SnakeGame.Services;

namespace SnakeGame.Factories
{
    public class Level2Factory : ILevelFactory
    {
        public Obstacle generateObstacle()
        {
            Random rand = new Random();
            int next = rand.Next(3);
            switch (next)
            {
                case 0:
                    return new Tunnel();
                case 1:
                    return new Rock();
                case 2:
                    return new Room();
            }
            return new Rock();
        }

        public Consumable generateConsumable(GameInstance instance)
        {
            Random foodRand = new Random();
            Consumable fruit;

            switch (foodRand.Next(0, 10))
            {
                case >= 9:
                    fruit = new BigFruit(instance, new StrawberryAttributes());
                    break;
                case >= 7:
                    fruit = new Fruit(instance, new WatermelonAttributes()); 
                    break;
                case >= 4:
                    fruit = new Fruit(instance, new LemonAttributes());
                    break;
                default:
                    fruit = new Fruit(instance, new StrawberryAttributes());
                    break;
            }
            if (fruit is not BigFruit)
            {
                fruit.GenerateNewPosition();
            }
            return fruit;
        }

        public Map generateMap(GameInstance instance)
        {
            return new Level2Map(instance);
        }
    }
}
