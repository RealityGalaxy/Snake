using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
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
            int roll = foodRand.Next(0, 10);
            if (roll >= 9)
            {
                return new Watermelon(instance);
            }
            if (roll >= 6)
            {
                return new Lemon(instance);
            }
            Strawberry strawberry = new(instance);
            strawberry.GenerateNewPosition();
            return strawberry;
        }

        public Map generateMap(GameInstance instance)
        {
            return new Level1Map(instance);
        }
    }
}
