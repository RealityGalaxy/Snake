using SnakeGame.Services;
using System.Drawing;

namespace SnakeGame.Models.FactoryModels.Fruit
{
    public class Lemon : Consumable
    {
        public Lemon(GameInstance instance)
        {
            Value = 2;
            Color = "#FFFF00";
            Instance = instance;
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
