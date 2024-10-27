using SnakeGame.Services;
using System.Drawing;

namespace SnakeGame.Models.FactoryModels.Fruit
{
    public class Fruit : Consumable
    {

        public Fruit(GameInstance instance, FruitAttributes attributes)
        {
            Instance = instance;
            Attributes = attributes;
        }

        public override bool CanConsume()
        {
            throw new NotImplementedException();
        }

        public override int Consume()
        {
            return Attributes.Value;
        }
    }
}
