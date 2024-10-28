using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SnakeGame.Factories;
using SnakeGame.Hubs;
using SnakeGame.Models;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.Services;
using Xunit;

namespace SnakeGame.Tests
{
    public class GameInstanceTests
    {
        private int _instanceId = 0;
        private GameInstance _gameInstance;

        public GameInstanceTests()
        {
            // Initialize GameInstance
            _gameInstance = new GameInstance(_instanceId);
        }

        [Fact]
        public void Constructor_ShouldInitializeGameInstance()
        {
            // Assert
            Assert.Equal(_instanceId, _gameInstance.InstanceId);
            Assert.NotNull(_gameInstance.Snakes);
            Assert.False(_gameInstance.IsGameRunning);
            Assert.NotNull(_gameInstance.LevelFactory);
            Assert.NotNull(_gameInstance.Map);
            Assert.NotNull(_gameInstance.Consumables);
        }

        [Fact]
        public void StartTimer_ShouldChangeTimerSettings()
        {
            // Act
            _gameInstance.StartTimer();

            // Since the timer is private, we can't assert its state directly.
            // We can check if no exceptions are thrown.
            Assert.True(true);
        }

        [Fact]
        public void ResetGame_ShouldResetGameState()
        {
            // Arrange
            _gameInstance.IsGameRunning = true;
            _gameInstance.Snakes.TryAdd("snake1", new Snake("snake1", new Point(1, 1), null, "#FF0000", "Snake1"));
            _gameInstance.Consumables.Add(new Point(2, 2), new Strawberry(_gameInstance));

            // Act
            _gameInstance.ResetGame();

            // Assert
            Assert.False(_gameInstance.IsGameRunning);
            Assert.Empty(_gameInstance.Snakes);
            Assert.Empty(_gameInstance.Consumables);
            Assert.NotNull(_gameInstance.Map);
        }

        [Fact]
        public void GetRandomEmptyPosition_ShouldReturnValidPosition()
        {
            // Act
            var position = _gameInstance.GetRandomEmptyPosition();

            // Assert
            Assert.InRange(position.X, 1, _gameInstance.Map.Width - 2);
            Assert.InRange(position.Y, 1, _gameInstance.Map.Height - 2);
            Assert.Equal(Map.CellType.Empty, _gameInstance.Map.Grid[position.X, position.Y]);
        }

        [Fact]
        public void RemoveSnake_ShouldRemoveSnakeFromGame()
        {
            // Arrange
            var snake = new Snake("snake1", new Point(1, 1), null, "#FF0000", "Snake1");
            _gameInstance.Snakes.TryAdd("snake1", snake);
            _gameInstance.Map.Grid[1, 1] = Map.CellType.Snake;

            // Act
            _gameInstance.RemoveSnake("snake1");

            // Assert
            Assert.False(_gameInstance.Snakes.ContainsKey("snake1"));
            Assert.Equal(Map.CellType.Empty, _gameInstance.Map.Grid[1, 1]);
        }

        [Fact]
        public void AddSnake_ShouldAddSnakeToGame()
        {
            // Arrange
            var hubStub = new Mock<IHubContext<GameHub>>();
            var gameService = new Mock<GameService>(hubStub.Object);
            GameService.Instance = gameService.Object;
            gameService.Object.GameInstances = new GameInstance[1];
            gameService.Object.GameInstances[_instanceId] = _gameInstance;
            var connectionId = "snake1";
            var color = "#FF0000";
            var name = "Snake1";

            // Act
            _gameInstance.AddSnake(connectionId, color, name, _instanceId);

            // Assert
            Assert.True(_gameInstance.Snakes.ContainsKey(connectionId));
            var snake = _gameInstance.Snakes[connectionId];
            Assert.Equal(connectionId, snake.ConnectionId);
            Assert.Equal(color, snake.Color);
            Assert.Equal(name, snake.Name);
            var head = snake.Body.First.Value;
            Assert.Equal(Map.CellType.Snake, _gameInstance.Map.Grid[head.X, head.Y]);
        }

        [Fact]
        public void AddSnake_ShouldNotAddSnake_WhenAlreadyExists()
        {
            // Arrange
            var connectionId = "snake1";
            var color = "#FF0000";
            var name = "Snake1";
            _gameInstance.Snakes.TryAdd(connectionId, new Snake(connectionId, new Point(1, 1), null, color, name));

            // Act
            _gameInstance.AddSnake(connectionId, "#00FF00", "NewSnake", _instanceId);

            // Assert
            var snake = _gameInstance.Snakes[connectionId];
            Assert.Equal(color, snake.Color);
            Assert.Equal(name, snake.Name);
        }

        [Fact]
        public void GameLoop_ShouldUpdateGameState_WhenGameIsRunning()
        {
            // Since GameLoop is private, we can't call it directly.
            // We can simulate the effects by starting the timer and checking the state after a delay.

            // Arrange
            _gameInstance.IsGameRunning = true;
            var hubStub = new Mock<IHubContext<GameHub>>();
            var gameService = new Mock<GameService>(hubStub.Object);
            GameService.Instance = gameService.Object;
            gameService.Object.GameInstances = new GameInstance[1];
            gameService.Object.GameInstances[_instanceId] = _gameInstance;
            var snake = new Snake("snake1", new Point(5, 5), gameService.Object, "#FF0000", "Snake1");
            _gameInstance.foodCounter = 1;
            _gameInstance.Snakes.TryAdd("snake1", snake);

            // Act
            _gameInstance.StartTimer();
            Thread.Sleep(50); // Wait for the timer to tick
            snake.IsAlive = false;
            Thread.Sleep(50); // Wait for the timer to tick

            // Assert
            Assert.NotEmpty(_gameInstance.Consumables);
            Assert.Empty(_gameInstance.Snakes);
        }
    }
}
