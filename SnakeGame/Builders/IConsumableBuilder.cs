using SnakeGame.Models;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit.Attributes;

namespace SnakeGame.Builders
{
    public interface IConsumableBuilder
    {
        IConsumableBuilder SetPosition(Point position);
        IConsumableBuilder SetColor(string color);
        IConsumableBuilder SetValue(int value);
        IConsumableBuilder SetType(Type consumableType);
        IConsumableBuilder SetPoison(bool isPoison);
        IConsumableBuilder SetDynamicPositioning(bool isDynamic);
        IConsumableBuilder SetBigConsumable(bool isBig);
        IConsumableBuilder SetAttributes(FruitAttributes attributes);
        Consumable Build();
    }
}
