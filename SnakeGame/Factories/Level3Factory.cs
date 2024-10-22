using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.Services;

namespace SnakeGame.Factories
{
    public class Level3Factory : ILevelFactory
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
            int roll = foodRand.Next(0, 10);
            if (roll >= 9)
            {
                return new RainbowFruit(instance);
            }
            if (roll >= 7)
            {
                return new BigApple(instance);
            }
            if (roll >= 5)
            {
                return new Watermelon(instance);
            }
            return new Lemon(instance);
        }

        public Map generateMap(GameInstance instance)
        {
            return new Level3Map(instance);
        }
    }
}
