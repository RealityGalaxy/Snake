using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.Services;
using SnakeGame.Builders;

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
            ConsumableBuilder builder = new(instance);
            Random foodRand = new Random();
            int roll = foodRand.Next(0, 10);
            int poisonRoll = foodRand.Next(0, 10);
            int dynamicRoll = foodRand.Next(0, 10);
            if (roll >= 9)
            {
                builder.SetType(typeof(Watermelon));
            }
            else if (roll >= 6)
            {
                builder.SetType(typeof(Lemon));
            }
            else
            {
                builder.SetType(typeof(Strawberry));
            }
            if (poisonRoll >= 9)
            {
                builder.SetPoison(true);
            }
            if (dynamicRoll >= 7)
            {
                builder.SetDynamicPositioning(true);
            }
            return builder.Build();
        }

        public Map generateMap(GameInstance instance)
        {
            return new Level1Map(instance);
        }
    }
}
