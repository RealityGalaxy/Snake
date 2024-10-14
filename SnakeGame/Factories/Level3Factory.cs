using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;

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

        public Consumable generateConsumable()
        {
            Random foodRand = new Random();
            int roll = foodRand.Next(0, 10);
            if (roll >= 9)
            {
                return new RainbowFruit();
            }
            if (roll >= 7)
            {
                return new BigApple();
            }
            if (roll >= 5)
            {
                return new Watermelon();
            }
            return new Lemon();
        }

        public Map generateMap()
        {
            return new Level3Map();
        }
    }
}
