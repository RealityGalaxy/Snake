namespace SnakeGame.Models.FactoryModels
{
    public interface IPrototype<T>
    {
        T Clone();
    }
}
