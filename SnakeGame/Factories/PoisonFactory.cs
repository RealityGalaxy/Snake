using SnakeGame.Models.Consumables;
using SnakeGame.Models.Consumables.Fruit;

namespace SnakeGame.Factories
{
    public class PoisonFactory : IConsumableFactory
    {
        public SmallConsumable generateSmallConsumable()
        {
            return new WeakPoison();
        }
        public MediumConsumable generateMediumConsumable()
        {
            return new Poison();
        }
        public BigConsumable generateBigConsumable()
        {
            return new StrongPoison();
        }
    }
}
