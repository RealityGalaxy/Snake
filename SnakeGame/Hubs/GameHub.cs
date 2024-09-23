using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SnakeGame.Models;
using SnakeGame.Services;

namespace SnakeGame.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameService _gameService;

        public GameHub(GameService gameService)
        {
            _gameService = gameService;
        }

        public async Task SendDirection(string direction)
        {
            // Update the snake's direction
            if (_gameService.Snakes.TryGetValue(Context.ConnectionId, out Snake snake))
            {
                snake.Turn(Enum.Parse<Snake.Direction>(direction, true));
            }
        }

        public async Task StartGame()
        {
            _gameService.IsGameRunning = true;
            _gameService.StartTimer();
            await Clients.All.SendAsync("GameStarted");
        }

        public async Task ResetGame()
        {
            _gameService.ResetGame();
            await Clients.All.SendAsync("GameReset");
        }

        public async Task AddSnake(string color, string name)
        {
            var path = Context.GetHttpContext().Request.Path;

            if (path.Value.Contains("gamehub"))
            {
                // Add a new snake for the connected client
                var startPosition = _gameService.GetRandomEmptyPosition();
                var snake = new Snake(Context.ConnectionId, startPosition, _gameService, color, name);
                _gameService.AddSnake(snake);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _gameService.RemoveSnake(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
