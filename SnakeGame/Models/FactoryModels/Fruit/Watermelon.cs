using SnakeGame.Services;
using System.Drawing;

namespace SnakeGame.Models.FactoryModels.Fruit
{
    public class Watermelon : Consumable
    {
        public Watermelon(GameInstance instance)
        {
            Instance = instance;
            GenerateNewPosition();
            Value = 3;
            Color = "#00FF00";
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
