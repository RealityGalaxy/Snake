using System.Drawing;

namespace SnakeGame.Models.Consumables.Fruit
{
    public class WeakPoison : SmallConsumable
    {
        public WeakPoison()
        {
            Value = -1;
            Color = "#18B800";
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
