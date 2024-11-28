namespace SnakeGame.Handlers
{
    public class CollisionResult
    {
        public bool KillSnake { get; set; } = false;
        public bool GrowSnake { get; set; } = false;
        public int ShrinkSnake { get; set; } = 0;
    }

}
