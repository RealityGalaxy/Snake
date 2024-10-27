using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.SignalR;
using SnakeGame.Commands;
using SnakeGame.Factories;
using SnakeGame.Models;
using SnakeGame.Services;

namespace SnakeGame.Hubs
{
    public class GameHub : Hub
    {
        private GameService _gameService;
        private CommandManager _commandManager;

        public GameHub(GameService gameService, CommandManager commandManager)
        {
            _gameService = gameService;
            _commandManager = commandManager;
        }

        public async Task Undo(int instance)
        {
            _commandManager.Undo(instance);
        }
        public async Task Pause(int instance)
        {
            _commandManager.ExecuteCommand(CommandManager.Pause, instance);
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
            _commandManager.ExecuteCommand(CommandManager.Start, instance);
            await Clients.Clients(_gameService.GetSubscribersForInstance(instance)).SendAsync("GameStarted");
        }

        public async Task ResetGame(int level, int instance)
        {
            _commandManager.ExecuteCommand(CommandManager.Generate, instance, new Dictionary<string, string> { { "level", level.ToString() } });
            await Clients.Clients(_gameService.GetSubscribersForInstance(instance)).SendAsync("GameReset");
        }

        public async Task AddSnake(string color, string name, int instance)
        {
            _commandManager.ExecuteCommand(CommandManager.Join, instance, new Dictionary<string, string> { { "connectionId", Context.ConnectionId }, { "color", color }, { "name", name } });
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
