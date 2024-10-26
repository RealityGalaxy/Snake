using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.Services;
using SnakeGame.Models;

namespace SnakeGame.Builders
{
    public class ConsumableBuilder
    {
        private GameInstance _instance;
        private Point? _position; // Make _position nullable
        private string _color = "DefaultColor";
        private int _value = 0;
        private Type _consumableType;
        private bool _isPoison = false;

        public ConsumableBuilder(GameInstance instance)
        {
            _instance = instance;
        }

        public ConsumableBuilder SetPosition(Point position)
        {
            _position = position;
            return this;
        }

        public ConsumableBuilder SetColor(string color)
        {
            _color = color;
            return this;
        }

        public ConsumableBuilder SetValue(int value)
        {
            _value = value;
            return this;
        }

        public ConsumableBuilder SetType(Type consumableType)
        {
            _consumableType = consumableType;
            return this;
        }
        public ConsumableBuilder SetPoison(bool isPoison)
        {
            _isPoison = isPoison;
            return this;
        }

        public Consumable Build()
        {
            if (_consumableType == null) throw new InvalidOperationException("Consumable type must be set");

            Consumable consumable = (Consumable)Activator.CreateInstance(_consumableType, _instance);
            if (_position.HasValue) consumable.Position = _position.Value;
            consumable.Color = _isPoison ? "Purple" : consumable.Color; // Change color if poisonous
            consumable.Value = _value == 0 ? consumable.Value : _value;
            consumable.IsPoisonous = _isPoison;
            if (_isPoison) consumable.Value = consumable.Value * -1;
            return consumable;
        }
    }
}
