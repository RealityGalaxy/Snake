using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using SnakeGame.Hubs;
using SnakeGame.Models;

namespace SnakeGame.Services
{
    public class GameService : BackgroundService
    {
        private readonly IHubContext<GameHub> _hubContext;
        private Timer _timer;

        public ConcurrentDictionary<string, Snake> Snakes { get; } = new();
        public bool IsGameRunning { get; set; } = false;
        public Fruit Fruit { get; private set; }

        public GameService(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
            Fruit = new Fruit();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Start the game loop timer
            _timer.Change(0, 300); // Update every 200ms
            return Task.CompletedTask;
        }

        public void StartTimer()
        {
            if (_timer != null)
            {
                _timer.Change(0, 300);
            }
        }

        private void GameLoop(object state)
        {
            BroadcastGameState();
            if (IsGameRunning)
            {
                Console.WriteLine("Game loop is running...");
                UpdateGameState();
            }
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
            Map.Instance.Grid[head.X, head.Y] = Map.CellType.Snake;
        }

        public void RemoveSnake(string connectionId)
        {
            if (Snakes.TryRemove(connectionId, out Snake snake))
            {
                // Clear the snake's body from the map
                foreach (var segment in snake.Body)
                {
                    Map.Instance.Grid[segment.X, segment.Y] = Map.CellType.Empty;
                }
            }
        }

        public void ResetGame()
        {
            IsGameRunning = false;
            Snakes.Clear();
            Map.Instance.InitializeGrid();
            Fruit = new Fruit();
        }

        public Point GetRandomEmptyPosition()
        {
            var random = new Random();
            int x, y;

            do
            {
                x = random.Next(1, Map.Instance.Width - 2);
                y = random.Next(1, Map.Instance.Height - 2);
            } while (Map.Instance.Grid[x, y] != Map.CellType.Empty);

            return new Point(x, y);
        }

        private object GetGameState()
        {
            // Construct the game state object to send to clients
            var walls = new List<object>();
            for (int x = 0; x < Map.Instance.Width; x++)
            {
                for (int y = 0; y < Map.Instance.Height; y++)
                {
                    if (Map.Instance.Grid[x, y] == Map.CellType.Wall)
                    {
                        walls.Add(new { x, y });
                    }
                }
            }

            var fruits = new List<object>
            {
                new { x = Fruit.Position.X, y = Fruit.Position.Y }
            };

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
