using SnakeGame.Models;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Composites;

namespace SnakeGame.Visitor
{
    public interface IVisitor
    {
        void Visit(MovableComposite composite);
        void Visit(Snake snake);
        void Visit(Consumable consumable);
    }
}
