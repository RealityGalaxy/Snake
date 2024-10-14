using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;

namespace SnakeGame.Factories
{
    public class Level1Factory : ILevelFactory
    {
        public Obstacle generateObstacle()
        {
            return new SmallRock();
        }

        public Consumable generateConsumable()
        {
            Random foodRand = new Random();
            int roll = foodRand.Next(0, 10);
            if (roll >= 9)
            {
                return new Watermelon();
            }
            if (roll >= 6)
            {
                return new Lemon();
            }
            Strawberry strawberry = new();
            strawberry.GenerateNewPosition();
            return strawberry;
        }

        public Map generateMap()
        {
            return new Level1Map();
        }
    }
}
