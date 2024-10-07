using System.Drawing;

namespace SnakeGame.Models.Consumables.Fruit
{
    public class StrongPoison : BigConsumable
    {
        public StrongPoison()
        {
            Value = -3;
            Color = "#0B5200";
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
