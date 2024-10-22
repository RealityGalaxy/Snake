using SnakeGame.Services;
using System.Drawing;

namespace SnakeGame.Models.FactoryModels.Fruit
{
    public class Strawberry : Consumable
    {
        public Strawberry(GameInstance instance)
        {
            Instance = instance;
            Value = 1;
            Color = "#FF0000";
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
