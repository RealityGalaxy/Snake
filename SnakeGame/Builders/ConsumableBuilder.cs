using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit.Attributes;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.Services;
using SnakeGame.Models;

namespace SnakeGame.Builders
{
    public class ConsumableBuilder : IConsumableBuilder
    {
        private GameInstance _instance;
        private Point? _position;
        private string _color = "DefaultColor";
        private int _value = 0;
        private Type _consumableType;
        private bool _isPoison = false;
        private bool _isDynamic = false;
        private bool _isBig = false;

        public ConsumableBuilder(GameInstance instance)
        {
            _instance = instance;
        }

        public IConsumableBuilder SetPosition(Point position)
        {
            _position = position;
            return this;
        }

        public IConsumableBuilder SetColor(string color)
        {
            _color = color;
            return this;
        }

        public IConsumableBuilder SetValue(int value)
        {
            _value = value;
            return this;
        }

        public IConsumableBuilder SetType(Type consumableType)
        {
            _consumableType = consumableType;
            return this;
        }

        public IConsumableBuilder SetPoison(bool isPoison)
        {
            _isPoison = isPoison;
            return this;
        }

        public IConsumableBuilder SetDynamicPositioning(bool isDynamic)
        {
            _isDynamic = isDynamic;
            return this;
        }

        public IConsumableBuilder SetBigConsumable(bool isBig)
        {
            _isBig = isBig;
            return this;
        }
        public IConsumableBuilder SetAttributes(FruitAttributes attributes)
        {
            _value = attributes.Value;
            _color = attributes.Color;
            return this;
        }
        public Consumable Build()
        {
            if (_consumableType == null)
                _consumableType = typeof(Fruit);

            Consumable consumable;

            switch (_consumableType)
            {
                case Type bigFruitType when bigFruitType == typeof(BigFruit):
                    consumable = new BigFruit(_instance, new FruitAttributes());
                    break;

                case Type rainbowFruitType when rainbowFruitType == typeof(RainbowFruit):
                    consumable = new RainbowFruit(_instance, new FruitAttributes());
                    break;

                default:
                    consumable = new Fruit(_instance, new FruitAttributes());
                    break;
            }
            if (_position.HasValue) consumable.Position = _position.Value;
            consumable.Attributes.Color = _isPoison ? "Purple" : _color;
            consumable.Attributes.Value = _value == 0 ? consumable.Attributes.Value : _value;
            consumable.IsPoisonous = _isPoison;
            if (_isPoison) consumable.Attributes.Value = consumable.Attributes.Value * -1;
            consumable.IsDynamic = _isDynamic;
            consumable.IsBigConsumable = _isBig;

            if (_isBig)
            {
                for (int i = 0; i <= 1; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        Point position = new Point(consumable.Position.X + i, consumable.Position.Y + j);
                        Consumable bigConsumable = consumable.Clone();
                        bigConsumable.Position = position;
                        _instance.Map.Grid[position.X, position.Y] = Map.CellType.Consumable;
                        _instance.Consumables.Add(position, bigConsumable);
                    }
                }
            }
            else
            {
                _instance.Consumables.Add(consumable.Position, consumable);
            }

            return consumable;
        }
    }
}
