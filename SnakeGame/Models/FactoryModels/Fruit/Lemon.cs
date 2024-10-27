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


        // Implementing the Clone method
        public override Consumable Clone()
        {
            // Create a new Lemon instance
            Lemon clone = new Lemon(Instance)
            {
                Position = this.Position,
                Value = this.Value,
                Color = this.Color,
                IsPoisonous = this.IsPoisonous,
                IsDynamic = this.IsDynamic
            };

            return clone;
        }
    }
}
