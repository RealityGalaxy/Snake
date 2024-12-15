using SnakeGame.Visitor;

namespace SnakeGame.Composites
{
    public interface IMovable
    {
        void Move();
        void GenerateNewPosition();
        void Accept(IVisitor visitor);
    }
}
