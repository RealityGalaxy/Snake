using SnakeGame.Builders;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.Models.FactoryModels.Fruit.Attributes;
using SnakeGame.Models.FactoryModels.Maps.MapSizes;
using SnakeGame.Services;

namespace SnakeGame.Factories
{
    public class Level2Factory : ILevelFactory
    {
        private readonly Obstacle _tunnelPrototype = new Tunnel();
        private readonly Obstacle _rockPrototype = new Rock();
        private readonly Obstacle _roomPrototype = new Room();

        public Obstacle generateObstacle()
        {
            Random rand = new Random();
            int next = rand.Next(3);

            switch (next)
            {
                case 0:
                    return _tunnelPrototype.Clone();
                case 1:
                    return _rockPrototype.Clone();
                case 2:
                    return _roomPrototype.Clone();
                default:
                    return _rockPrototype.Clone();
            }
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
                    builder.SetType(typeof(BigFruit))
                        .SetAttributes(new StrawberryAttributes())
                        .SetBigConsumable(true)
                        .SetDynamicPositioning(false)
                        .SetPoison(false);
                    return builder.Build();
                case >= 7:
                    builder.SetAttributes(new WatermelonAttributes());
                    break;
                case >= 4:
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
            return new Level2Map(instance, new MapSize30());
        }
    }
}
