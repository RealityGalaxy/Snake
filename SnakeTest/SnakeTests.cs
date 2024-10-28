using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SnakeGame.Hubs;
using SnakeGame.Models;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Fruit;
using SnakeGame.Services;
using SnakeGame.Strategies;
using Xunit;

namespace SnakeGame.Tests
{
    public class SnakeTests
    {
        private readonly Mock<GameService> _mockGameService;
        private GameInstance _gameInstance;
        private string _connectionId = "test_connection";
        private string _color = "#FF0000";
        private string _name = "TestSnake";
        private int _instanceId = 0;

        public SnakeTests()
        {
            var hubStub = new Mock<IHubContext<GameHub>>();
            _mockGameService = new Mock<GameService>(hubStub.Object);
            GameService.Instance = _mockGameService.Object;
            _gameInstance = new GameInstance(_instanceId);
            _mockGameService.Object.GameInstances = new GameInstance[1];
            _mockGameService.Object.GameInstances[_instanceId] = _gameInstance;
            _mockGameService.Object.Subscribers = new List<Subscriber>()
            {
                new Subscriber(_connectionId) { InstanceNumber = _instanceId }
            };
        }

        [Fact]
        public void Constructor_ShouldInitializeSnake()
        {
            // Arrange
            var startPosition = new Point(5, 5);

            // Act
            var snake = new Snake(_connectionId, startPosition, _mockGameService.Object, _color, _name);

            // Assert
            Assert.Equal(_connectionId, snake.ConnectionId);
            Assert.Equal(startPosition, snake.Body.First.Value);
            Assert.Equal(Snake.Direction.Right, snake.CurrentDirection);
            Assert.Equal(_color, snake.Color);
            Assert.Equal(_name, snake.Name);
            Assert.True(snake.IsAlive);
            Assert.IsType<BasicStrategy>(snake.CurrentStrategy);
        }

        [Theory]
        [InlineData(Snake.Direction.Up, Snake.Direction.Left, Snake.Direction.Left)]
        [InlineData(Snake.Direction.Up, Snake.Direction.Right, Snake.Direction.Right)]
        [InlineData(Snake.Direction.Left, Snake.Direction.Up, Snake.Direction.Up)]
        [InlineData(Snake.Direction.Left, Snake.Direction.Down, Snake.Direction.Down)]
        [InlineData(Snake.Direction.Down, Snake.Direction.Left, Snake.Direction.Left)]
        [InlineData(Snake.Direction.Down, Snake.Direction.Right, Snake.Direction.Right)]
        [InlineData(Snake.Direction.Right, Snake.Direction.Up, Snake.Direction.Up)]
        [InlineData(Snake.Direction.Right, Snake.Direction.Down, Snake.Direction.Down)]
        [InlineData(Snake.Direction.None, Snake.Direction.Down, Snake.Direction.Down)] // When no direction present
        [InlineData(Snake.Direction.Up, Snake.Direction.Down, Snake.Direction.Up)]    // Invalid turn
        [InlineData(Snake.Direction.Left, Snake.Direction.Right, Snake.Direction.Left)] // Invalid turn
        public void Turn_ShouldChangeDirection_WhenValid(Snake.Direction current, Snake.Direction newDirection, Snake.Direction expected)
        {
            // Arrange
            var snake = new Snake(_connectionId, new Point(5, 5), _mockGameService.Object, _color, _name)
            {
                CurrentDirection = current
            };

            // Act
            snake.Turn(newDirection);

            // Assert
            Assert.Equal(expected, snake.CurrentDirection);
        }

        [Fact]
        public void Turn_ShouldNotChangeDirection_WhenSnakeIsDead()
        {
            // Arrange
            var snake = new Snake(_connectionId, new Point(5, 5), _mockGameService.Object, _color, _name)
            {
                IsAlive = false,
                CurrentDirection = Snake.Direction.Up
            };

            // Act
            snake.Turn(Snake.Direction.Down);

            // Assert
            Assert.Equal(Snake.Direction.Up, snake.CurrentDirection);
        }

        [Fact]
        public void Move_ShouldNotMove_WhenSnakeIsDead()
        {
            // Arrange
            var snake = new Snake(_connectionId, new Point(5, 5), _mockGameService.Object, _color, _name)
            {
                IsAlive = false
            };

            // Act
            snake.Move();

            // Assert
            Assert.Single(snake.Body);
        }

        [Fact]
        public void Move_ShouldDecreaseMoveTimer_WhenTimerIsNotZero()
        {
            // Arrange
            var snake = new Snake(_connectionId, new Point(5, 5), _mockGameService.Object, _color, _name)
            {
                MoveTimer = 3
            };

            // Act
            snake.Move();

            // Assert
            Assert.Equal(2, snake.MoveTimer);
            Assert.Single(snake.Body);
        }



        
        [Fact]
        public void Move_ShouldChangeStrategyToBasic_WhenRainbowTimerExpires()
        {
            // Arrange
            var snake = new Snake(_connectionId, new Point(5, 5), _mockGameService.Object, _color, _name)
            {
                RainbowTimer = 0,
                CurrentStrategy = new FastStrategy()
            };
            snake.MoveTimer = 0; // Ensure the snake moves

            // Act
            snake.Move();

            // Assert
            Assert.IsType<BasicStrategy>(snake.CurrentStrategy);
        }

        [Fact]
        public void Move_ShouldDie_WhenCollisionOccurs()
        {
            // Arrange
            var snake = new Snake(_connectionId, new Point(1, 1), _mockGameService.Object, _color, _name);
            _gameInstance.Map.Grid[2, 1] = Map.CellType.Wall;
            snake.CurrentDirection = Snake.Direction.Right;
            snake.MoveTimer = 0; // Ensure the snake moves

            // Act
            snake.Move();

            // Assert
            Assert.False(snake.IsAlive);
        }

        [Fact]
        public void Move_ShouldEatFood_AndGrow()
        {
            // Arrange
            var snake = new Snake(_connectionId, new Point(5, 5), _mockGameService.Object, _color, _name);
            _gameInstance.Snakes[_connectionId] = snake;
            var foodPosition = new Point(6, 5);
            var food = new Strawberry(_gameInstance)
            {
                Position = foodPosition
            };
            _gameInstance.Consumables[foodPosition] = food;
            _gameInstance.Map.Grid[6, 5] = Map.CellType.Consumable;
            snake.CurrentDirection = Snake.Direction.Right;

            // Ensure the snake moves
            snake.MoveTimer = 0;

            // Act
            snake.Move(); // Eats food

            // Prepare for next move
            snake.MoveTimer = 0;
            snake.Move(); // Grows

            // Assert
            Assert.Equal(2, snake.Body.Count);
            Assert.DoesNotContain(foodPosition, _gameInstance.Consumables.Keys);
        }


        [Fact]
        public void Move_ShouldChangeToFastStrategy_WhenEatingRainbowFruit()
        {
            // Arrange
            var snake = new Snake(_connectionId, new Point(5, 5), _mockGameService.Object, _color, _name);
            _gameInstance.Snakes[_connectionId] = snake;
            var foodPosition = new Point(6, 5);
            var food = new RainbowFruit(_gameInstance)
            {
                Position = foodPosition
            };
            _gameInstance.Consumables[foodPosition] = food;
            _gameInstance.Map.Grid[6, 5] = Map.CellType.Consumable;
            snake.CurrentDirection = Snake.Direction.Right;
            snake.MoveTimer = 0; // Ensure the snake moves

            // Act
            snake.Move(); // Eats RainbowFruit

            // Assert
            Assert.IsType<FastStrategy>(snake.CurrentStrategy);
            Assert.Equal(20, snake.RainbowTimer);
        }

        [Theory]
        [InlineData(Snake.Direction.Up, 5, 4)]
        [InlineData(Snake.Direction.Left, 4, 5)]
        [InlineData(Snake.Direction.Right, 6, 5)]
        [InlineData(Snake.Direction.Down, 5, 6)]
        [InlineData(Snake.Direction.None, 5, 5)]
        public void Move_ShouldMoveInDirection_WhenDirectionIsGiven(Snake.Direction direction, int nextX, int nextY)
        {
            // Arrange
            var snake = new Snake(_connectionId, new Point(5, 5), _mockGameService.Object, _color, _name);
            snake.CurrentDirection = direction;
            snake.MoveTimer = 0;
            var nextPosition = new Point(nextX, nextY);

            // Act
            snake.Move();

            // Assert
            Assert.Equal(nextPosition, snake.Body.First.Value);
        }

        [Fact]
        public void Move_ShouldNotChangeDirection_WhenStrategyDoesNotReset()
        {
            // Arrange
            var snake = new Snake(_connectionId, new Point(5, 5), _mockGameService.Object, _color, _name);

            // Act
            snake.Move();

            // Assert
            Assert.NotEqual(Snake.Direction.None, snake.CurrentDirection);
        }

        [Fact]
        public void Move_ShouldChangeDirectionToNone_WhenStrategyResets()
        {
            // Arrange
            var snake = new Snake(_connectionId, new Point(5, 5), _mockGameService.Object, _color, _name)
            {
                MoveTimer = 0,
                CurrentStrategy = new ManualStrategy()
            };

            // Act
            snake.Move();

            // Assert
            Assert.Equal(Snake.Direction.None, snake.CurrentDirection);
        }

        // Helper method to access private fields
        private T GetPrivateField<T>(object obj, string fieldName)
        {
            var bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
            var field = obj.GetType().GetField(fieldName, bindingFlags);
            return (T)field.GetValue(obj);
        }
    }
}
