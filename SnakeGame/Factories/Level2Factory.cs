using SnakeGame.Builders;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
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
            if (roll >= 1)
            {
                builder.SetType(typeof(BigApple));
            }
            else if (roll >= 7)
            {
                builder.SetType(typeof(Watermelon));
            }
            else if (roll >= 4)
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
            return new Level2Map(instance);
        }
    }
}
