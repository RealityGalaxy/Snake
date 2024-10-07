using System.Drawing;

namespace SnakeGame.Models.Consumables.Fruit
{
    public class Strawberry : SmallConsumable
    {
        public Strawberry()
        {
            Value = 1;
            Color = "#FF0000";
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
