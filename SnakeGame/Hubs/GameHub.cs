using Microsoft.AspNetCore.SignalR;
using SnakeGame.Commands;
using SnakeGame.Models;
using SnakeGame.Services;
using System.Reflection.Emit;

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

        public async Task PauseGame(int instance)
        {
            _commandManager.ExecuteCommand(CommandManager.Pause, instance);
            await Clients.Clients(_gameService.GetSubscribersForInstance(instance)).SendAsync("GamePaused");
        }

        public async Task ResumeGame(int instance)
        {
            _commandManager.ExecuteCommand(CommandManager.Resume, instance);
            await Clients.Clients(_gameService.GetSubscribersForInstance(instance)).SendAsync("GameResumed");
        }

        public async Task EndGame(int instance)
        {
            _commandManager.ExecuteCommand(CommandManager.End, instance);
            await Clients.Clients(_gameService.GetSubscribersForInstance(instance)).SendAsync("GameEnded");
        }

        public async Task AddSnake(string color, string name, int instance, bool manual)
        {
            _commandManager.ExecuteCommand(CommandManager.Join, instance, new Dictionary<string, string> { { "connectionId", Context.ConnectionId }, { "color", color }, { "name", name }, { "manual", manual.ToString() } });
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
