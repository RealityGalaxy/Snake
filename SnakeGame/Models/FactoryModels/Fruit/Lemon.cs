using System.Drawing;

namespace SnakeGame.Models.FactoryModels.Fruit
{
    public class Lemon : Consumable
    {
        public Lemon()
        {
            Value = 2;
            Color = "#FFFF00";
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
