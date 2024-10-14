using System.Drawing;

namespace SnakeGame.Models.FactoryModels.Fruit
{
    public class Watermelon : Consumable
    {
        public Watermelon()
        {
            Value = 3;
            Color = "#00FF00";
            GenerateNewPosition();
        }

        public override bool CanConsume()
        {
            throw new NotImplementedException();
        }

        public override int Consume()
        {
            return Value;
        }
    }
}
