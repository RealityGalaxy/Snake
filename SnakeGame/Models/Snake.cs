using SnakeGame.Iterators;
using SnakeGame.Services;
using SnakeGame.Template;
using SnakeGame.Composites;

namespace SnakeGame.Models
{
    public class Snake : IMovable
    {
        public string ConnectionId { get; }
        public LinkedList<Point> Body { get; }
        public Direction CurrentDirection { get; set; }
        private string BaseColor { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public bool IsAlive { get; set; } = true;
        public int MoveTimer { get; set; } = 6;
        public MovementTemplate BaseMovement { get; set; }
        public MovementTemplate Movement { get; set; }
        private GameService _gameService;

        public int tempFood = 0;
        public int RainbowTimer = 0;
        public int SpawnTimer = 24;

        public Snake(string connectionId, Point startPosition, GameService gameService, string color, string name, MovementTemplate movement)
        {
            ConnectionId = connectionId;
            Body = new LinkedList<Point>();
            Body.AddFirst(startPosition);
            CurrentDirection = Direction.Right;
            _gameService = gameService;
            BaseColor = color;
            Color = BaseColor;
            Name = name;
            BaseMovement = movement;
            Movement = BaseMovement;
        }

        public void Turn(Direction direction)
        {
            if (!IsAlive) return;
            if(CurrentDirection == Direction.None)
            {
                CurrentDirection = direction;
                return;
            }
            if (Math.Abs((int)direction - (int)CurrentDirection) % 3 <= 1)
            {
                CurrentDirection = direction;
            }
        }

        public void Move()
        {
            Movement.Move(this);
        }

        private CancellationTokenSource _cancellationTokenSource;
        public async void StartRainbowing()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            // Define a list of colors for the rainbow effect
            string[] rainbowColors = new[] { "#FF0000", "#FF7F00", "#FFFF00", "#00FF00", "#0000FF", "#4B0082", "#9400D3" };
            int colorIndex = 0;

            try
            {
                // Run the color-changing loop while the fruit is alive
                while (true)
                {
                    // Change the fruit's color
                    Color = rainbowColors[colorIndex];

                    // Cycle through the colors
                    colorIndex = (colorIndex + 1) % rainbowColors.Length;

                    // Wait for a short delay before changing the color again (e.g., 300 ms)
                    await Task.Delay(50, token);
                }
            }
            catch (TaskCanceledException)
            {
                Color = BaseColor;
            }
        }

        // Method to stop the rainbow process (when the fruit is consumed or removed)
        public void StopRainbowing()
        {
            _cancellationTokenSource?.Cancel(); // Stop the async loop
        }

        public IIterator<Point> GetIterator()
        {
            return new PointIterator(Body);
        }

        public enum Direction
        {
            Up,
            Right,
            Down,
            Left,
            None
        }

        public void GenerateNewPosition()
        {
            // yeah no, this does nothing. The only reason this exists is to make the Composite unsafe
        }
    }
}
