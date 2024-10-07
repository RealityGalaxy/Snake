using System.Drawing;

namespace SnakeGame.Models.Consumables.Fruit
{
    public class Poison : MediumConsumable
    {
        public Poison() 
        {
            Value = -2;
            Color = "#159E00";
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
