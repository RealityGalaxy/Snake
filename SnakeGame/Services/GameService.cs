using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using SnakeGame.Factories;
using SnakeGame.Hubs;
using SnakeGame.Models;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;

namespace SnakeGame.Services
{
    public class GameService : BackgroundService
    {
        public static GameService Instance { get; private set; }
        private readonly IHubContext<GameHub> _hubContext;
        private Timer _timer;

        public ConcurrentDictionary<string, Snake> Snakes { get; } = new();
        public bool IsGameRunning { get; set; } = false;
        public ILevelFactory LevelFactory { get; set; }
        public Dictionary<Point, Consumable> Consumables { get; private set; }
        public Map Map { get; set; }

        public GameService(IHubContext<GameHub> hubContext)
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                return;
            }
            _hubContext = hubContext;
            Consumables = new Dictionary<Point, Consumable>();
            LevelFactory = new Level1Factory();
            Map = LevelFactory.generateMap();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Start the game loop timer
            _timer.Change(0, 50); // Update every 200ms
            return Task.CompletedTask;
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
                    Consumable food = LevelFactory.generateConsumable();
                    if(food is not BigApple)
                    {
                        Consumables.Add(food.Position, food);
                    }
                }
            }
            BroadcastGameState();
        }

        private void UpdateGameState()
        {
            foreach (var snake in Snakes.Values)
            {
                snake.Move();
            }

            // Remove snakes that are no longer alive
            foreach (var snake in Snakes.Values)
            {
                if (!snake.IsAlive)
                {
                    RemoveSnake(snake.ConnectionId);
                }
            }
        }

        private async void BroadcastGameState()
        {
            var gameState = GetGameState();
            await _hubContext.Clients.All.SendAsync("ReceiveGameState", gameState);
        }

        public void AddSnake(Snake snake)
        {
            Snakes.TryAdd(snake.ConnectionId, snake);
            // Mark the initial position on the map
            var head = snake.Body.First.Value;
            Map.Grid[head.X, head.Y] = Map.CellType.Snake;
        }

        public void RemoveSnake(string connectionId)
        {
            if (Snakes.TryRemove(connectionId, out Snake snake))
            {
                // Clear the snake's body from the map
                foreach (var segment in snake.Body)
                {
                    Map.Grid[segment.X, segment.Y] = Map.CellType.Empty;
                }
            }
        }

        public void ResetGame()
        {
            IsGameRunning = false;
            Snakes.Clear();
            Map = LevelFactory.generateMap();
            Consumables = new Dictionary<Point, Consumable>();
        }

        public Point GetRandomEmptyPosition()
        {
            var random = new Random();
            int x, y;

            do
            {
                x = random.Next(1, Map.Width - 2);
                y = random.Next(1, Map.Height - 2);
            } while (Map.Grid[x, y] != Map.CellType.Empty);

            return new Point(x, y);
        }

        private object GetGameState()
        {
            // Construct the game state object to send to clients
            var walls = new List<object>();
            for (int x = 0; x < Map.Width; x++)
            {
                for (int y = 0; y < Map.Height; y++)
                {
                    if (Map.Grid[x, y] == Map.CellType.Wall)
                    {
                        walls.Add(new { x, y });
                    }
                }
            }

            var fruits = Consumables.Values.ToList().ConvertAll(consumable => new { x = consumable.Position.X, y = consumable.Position.Y, color = consumable.Color } as object);

            var snakesList = new List<object>();
            foreach (var snake in Snakes.Values)
            {
                var body = new List<object>();
                foreach (var segment in snake.Body)
                {
                    body.Add(new { x = segment.X, y = segment.Y });
                }
                snakesList.Add(new
                {
                    id = snake.ConnectionId,
                    color = snake.Color,
                    name = snake.Name + " - " +  snake.Body.Count,
                    body
                });
            }

            return new
            {
                width = Map.Width,
                height = Map.Height,
                walls,
                fruits,
                snakes = snakesList
            };
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            // Start the timer when the service starts
            _timer = new Timer(GameLoop, null, Timeout.Infinite, Timeout.Infinite);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
