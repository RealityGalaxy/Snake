using SnakeGame.Models.FactoryModels.Fruit.Attributes;
using SnakeGame.Services;

namespace SnakeGame.Models.FactoryModels.Fruit
{
    public class Fruit : Consumable
    {

        public Fruit(GameInstance instance, FruitAttributes attributes)
        {
            Instance = instance;
            Attributes = attributes;
            GenerateNewPosition();
        }
        public Fruit(GameInstance instance, FruitAttributes attributes, Point position)
        {
            Instance = instance;
            Attributes = attributes;
            Position = position;
        }

        public override bool CanConsume()
        {
            throw new NotImplementedException();
        }

        public override int Consume()
        {
            return Attributes.Value;
        }


        // Implementing the Clone method
        public override Consumable Clone()
        {
            // Create a new Fruit instance
            Consumable clone = new Fruit(Instance, Attributes)
            {
                IsPoisonous = this.IsPoisonous,
                IsDynamic = this.IsDynamic,
                IsBigConsumable = this.IsBigConsumable
            };
            return clone;
        }
    }
}
