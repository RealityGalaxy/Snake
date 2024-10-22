using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SnakeGame.Factories;
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

        public async Task SendDirection(string direction, int instance)
        {
            // Update the snake's direction
            if (_gameService.GameInstances[instance].Snakes.TryGetValue(Context.ConnectionId, out Snake snake))
            {
                snake.Turn(Enum.Parse<Snake.Direction>(direction, true));
            }
        }

        public async Task ChangeInstance(int instance)
        {
            _gameService.Subscribe(Context.ConnectionId, instance);
        }

        public async Task StartGame(int instance)
        {
            _gameService.GameInstances[instance].IsGameRunning = true;
            await Clients.Clients(_gameService.GetSubscribersForInstance(instance)).SendAsync("GameStarted");
        }

        public async Task ResetGame(int level, int instance)
        {
            switch (level)
            {
                case 1:
                    _gameService.GameInstances[instance].LevelFactory = new Level1Factory();
                    break;
                case 2:
                    _gameService.GameInstances[instance].LevelFactory = new Level2Factory();
                    break;
                case 3:
                    _gameService.GameInstances[instance].LevelFactory = new Level3Factory();
                    break;
            }
            _gameService.GameInstances[instance].ResetGame();
            await Clients.Clients(_gameService.GetSubscribersForInstance(instance)).SendAsync("GameReset");
        }

        public async Task AddSnake(string color, string name, int instance)
        {
            _gameService.GameInstances[instance].AddSnake(Context.ConnectionId, color, name, instance);
        }

        public override async Task OnConnectedAsync()
        {
            _gameService.Subscribe(Context.ConnectionId, 0);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _gameService.GameInstances[_gameService.GetInstance(Context.ConnectionId)].RemoveSnake(Context.ConnectionId);
            _gameService.Unsubscribe(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
