using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
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
            int roll = foodRand.Next(0, 10);
            if (roll >= 9)
            {
                return new BigApple(instance);
            }
            if (roll >= 7)
            {
                return new Watermelon(instance);
            }
            if (roll >= 4)
            {
                return new Lemon(instance);
            }
            Strawberry strawberry = new(instance);
            strawberry.GenerateNewPosition();
            return strawberry;
        }

        public Map generateMap(GameInstance instance)
        {
            return new Level2Map(instance);
        }
    }
}
