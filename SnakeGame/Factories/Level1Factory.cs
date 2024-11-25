using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit.Attributes;
using SnakeGame.Models.FactoryModels.Maps.MapSizes;
using SnakeGame.Services;
using SnakeGame.Builders;

namespace SnakeGame.Factories
{
    public class Level1Factory : ILevelFactory
    {
        public Obstacle generateObstacle()
        {
            return new Obstacle("small_rock");
        }

        public Consumable generateConsumable(GameInstance instance)
        {
            ConsumableBuilder builder = new(instance);
            Random foodRand = new Random();
            int roll = foodRand.Next(0, 10);
            int poisonRoll = foodRand.Next(0, 10);
            int dynamicRoll = foodRand.Next(0, 10);
            switch (roll)
            {
                case >= 9:
                    builder.SetAttributes(new WatermelonAttributes());
                    break;
                case >= 6:
                    builder.SetAttributes(new LemonAttributes());
                    break;
                default:
                    builder.SetAttributes(new StrawberryAttributes());
                    break;
            }
            if (poisonRoll >= 8)
            {
                builder.SetPoison(true);
            }
            if (dynamicRoll >= 3)
            {
                builder.SetDynamicPositioning(true);
            }
            return builder.Build();
        }

        public Map generateMap(GameInstance instance)
        {
            return new Level1Map(instance, new MapSize20());
        }
    }
}
