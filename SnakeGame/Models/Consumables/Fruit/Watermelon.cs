using System.Drawing;

namespace SnakeGame.Models.Consumables.Fruit
{
    public class Watermelon : BigConsumable
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
