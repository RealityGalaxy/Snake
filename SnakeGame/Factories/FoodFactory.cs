using SnakeGame.Models.Consumables;
using SnakeGame.Models.Consumables.Fruit;

namespace SnakeGame.Factories
{
    public class FoodFactory : IConsumableFactory
    {
        public SmallConsumable generateSmallConsumable()
        {
            return new Strawberry();
        }
        public MediumConsumable generateMediumConsumable()
        {
            return new Lemon();
        }
        public BigConsumable generateBigConsumable()
        {
            return new Watermelon();
        }
    }
}
