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
            GenerateNewPosition();
        }

        public Strawberry(GameInstance instance, Point position)
        {
            Instance = instance;
            Value = 1;
            Color = "#FF0000";
            Position = position;
        }

        public override bool CanConsume()
        {
            throw new NotImplementedException();
        }

        public override int Consume()
        {
            return Value;
        }

        public override Consumable Clone()
        {
            Strawberry clone = new Strawberry(Instance)
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
