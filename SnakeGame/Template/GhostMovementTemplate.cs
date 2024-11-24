using SnakeGame.Models;

namespace SnakeGame.Template
{
    public class GhostMovementTemplate : MovementTemplate
    {
        protected override bool IsCollision(Point point, int instance)
        {
            return false;
        }
    }
}
