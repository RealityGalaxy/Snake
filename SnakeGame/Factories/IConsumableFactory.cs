using SnakeGame.Models.Consumables;

namespace SnakeGame.Factories
{
    public interface IConsumableFactory
    {
        public SmallConsumable generateSmallConsumable();
        public MediumConsumable generateMediumConsumable();
        public BigConsumable generateBigConsumable();
    }
}
