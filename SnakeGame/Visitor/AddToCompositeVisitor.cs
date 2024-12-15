using SnakeGame.Composites;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models;

namespace SnakeGame.Visitor
{
    public class AddToCompositeVisitor : IVisitor
    {
        private readonly MovableComposite _foodComposite;
        private readonly MovableComposite _snakeComposite;

        public AddToCompositeVisitor(MovableComposite composite, MovableComposite snakeComposite)
        {
            _foodComposite = composite;
            _snakeComposite = snakeComposite;
        }

        public void Visit(MovableComposite composite)
        {
            // Could choose dynamically where to add based on logic.
            // Not worth implementing with 2 composites.
        }

        public void Visit(Snake snake)
        {
            _snakeComposite.Add(snake);
        }

        public void Visit(Consumable consumable)
        {
            _foodComposite.Add(consumable);
        }
    }
}
