using SnakeGame.Factories;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models;
using SnakeGame.Adapters;
using SnakeGame.Iterators;
using SnakeGame.Template;
using SnakeGame.Proxies;

namespace SnakeGame.Services
{
    public class GameInstance
    {
        public int InstanceId { get; set; }
        private Timer _timer;
        public ILeaderboard _leaderboard;
        public Dictionary<string, Snake> Snakes { get; set; } = new();
        public bool IsGameRunning { get; set; } = false;
        public ILevelFactory LevelFactory { get; set; }
        public Dictionary<Point, Consumable> Consumables { get; set; }
        public Map Map { get; set; }
        public SoundPlayer SoundPlayer { get; set; } = new SoundPlayer();
        public GameInstance(int id)
        {
            InstanceId = id;
            _timer = new Timer(GameLoop, null, Timeout.Infinite, Timeout.Infinite);
            Consumables = new Dictionary<Point, Consumable>();
            LevelFactory = new Level1Factory();
            Map = LevelFactory.generateMap(this);
            _leaderboard = new HighscoreLeaderboardProxy();
        }
        public void StartTimer()
        {
            if (_timer != null)
            {
                _timer.Change(0, 50);
            }
        }


        private const int foodTimer = 120;
        private int foodCounter = foodTimer;
        private const int updateTimer = 1;
        private int updateCounter = updateTimer;
        private int foodUpdateCounter = foodTimer / 4;
        private void GameLoop(object state)
        {
            if (IsGameRunning)
            {
                updateCounter--;
                if (updateCounter == 0)
                {
                    updateCounter = updateTimer;
                    UpdateGameState();
                }
                foodCounter--;
                if (foodCounter == 0)
                {
                    foodCounter = foodTimer;
                    Consumable food = LevelFactory.generateConsumable(this);

                    var sound = SoundPlayer.PlaySound("fruit_spawn");
                    GameService.Instance.PlaySound(sound, InstanceId);
                }
                foodUpdateCounter--;
                if (foodUpdateCounter <= 0)
                {
                    foodUpdateCounter = foodTimer / 4;

                    Dictionary<Point, Consumable> newConsumables = new();

                    foreach (var consumable in Consumables.Values)
                    {
                        consumable.Move();
                        newConsumables.Add(consumable.Position, consumable);
                    }

                    Consumables = newConsumables;
                }
            }
            GameService.Instance.BroadcastGameState(GetGameState(), InstanceId);
        }


        private object GetGameState()
        {
            // Construct the game state object to send to clients
            var walls = new List<object>();
            for (int x = 0; x < Map.Size.Width; x++)
            {
                for (int y = 0; y < Map.Size.Height; y++)
                {
                    if (Map.Grid[x, y] == Map.CellType.Wall)
                    {
                        walls.Add(new { x, y });
                    }
                }
            }

            var fruits = Consumables.Values.ToList().ConvertAll(consumable => new { x = consumable.Position.X, y = consumable.Position.Y, color = consumable.Attributes.Color } as object);

            var snakesList = new List<object>();
            var snakeIterator = GetIterator();

            while (snakeIterator.HasNext())
            {
                var snake = snakeIterator.Next();
                if (snake == null)
                {
                    continue;
                }
                var body = new List<object>();

                var snakeBody = snake.GetIterator();

                while (snakeBody.HasNext())
                {
                    var segment = snakeBody.Next();
                    body.Add(new { x = segment.X, y = segment.Y });
                }
                snakesList.Add(new
                {
                    id = snake.ConnectionId,
                    color = snake.Color,
                    name = snake.Name + " - " + snake.Body.Count,
                    body
                });
            }

            return new
            {
                width = Map.Size.Width,
                height = Map.Size.Height,
                walls,
                fruits,
                snakes = snakesList
            };
        }
        public void ResetGame()
        {
            IsGameRunning = false;
            Snakes.Clear();
            Map = LevelFactory.generateMap(this);
            Consumables = new Dictionary<Point, Consumable>();
        }

        public Point GetRandomEmptyPosition()
        {
            var random = new Random();
            int x, y;

            do
            {
                x = random.Next(1, Map.Size.Width - 2);
                y = random.Next(1, Map.Size.Height - 2);
            } while (Map.Grid[x, y] != Map.CellType.Empty);

            return new Point(x, y);
        }

        public void RemoveSnake(string connectionId)
        {
            if (Snakes.Remove(connectionId, out Snake snake))
            {
                var bodyIterator = snake.GetIterator();

                // Clear the snake's body from the map
                while (bodyIterator.HasNext())
                {
                    var segment = bodyIterator.Next();
                    Map.Grid[segment.X, segment.Y] = Map.CellType.Empty;
                }

                var sound = SoundPlayer.PlaySound("death");
                GameService.Instance.PlaySound(sound, InstanceId);
            }
        }

        public void AddSnake(string connectionId, string color, string name, int instance, bool isManual)
        {
            MovementTemplate template = isManual ? new ManualMovementTemplate() : new BasicMovementTemplate();
            Snake snake = new Snake(connectionId, GetRandomEmptyPosition(), GameService.Instance, color, name, template);
            if(Snakes.TryAdd(snake.ConnectionId, snake))
            {
                // Mark the initial position on the map
                var head = snake.Body.First.Value;
                Map.Grid[head.X, head.Y] = Map.CellType.Snake;

                var sound = SoundPlayer.PlaySound("snake_spawn");
                GameService.Instance.PlaySound(sound, InstanceId);
            }
        }

        public IIterator<Snake> GetIterator() {
            return new SnakeIterator(Snakes);
        }

        private void UpdateGameState()
        {
            var snakeIterator = GetIterator();

            while (snakeIterator.HasNext())
            {
                var snake = snakeIterator.Next();
                if (snake == null)
                {
                    continue;
                }
                snake.Move();
            }

            snakeIterator = GetIterator();

            while (snakeIterator.HasNext())
            {
                var snake = snakeIterator.Next();
                if (snake == null)
                {
                    continue;
                }
                if (!snake.IsAlive)
                {
                    int score = snake.Body.Count;
                    if (_leaderboard.IsHighScore(score))
                    {
                        _leaderboard.AddScore(score, snake.Name);
                        GameService.Instance.UpdateGlobalLeaderboard();

                    }
                    RemoveSnake(snake.ConnectionId);
                }
            }
        }
    }
}
