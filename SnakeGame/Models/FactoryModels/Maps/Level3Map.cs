﻿using SnakeGame.Services;

namespace SnakeGame.Models.FactoryModels
{
    public class Level2Map : Map
    {
        public Level2Map() : base(30, 30)
        {
            GenerateObstacles(5);
        }

        public void GenerateObstacles(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Obstacle obstacle = GameService.Instance.LevelFactory.generateObstacle();
                Point position = GetRandomEmptyPosition();
                obstacle.Place(position, this);
            }
        }

        public Point GetRandomEmptyPosition()
        {
            var random = new Random();
            int x, y;

            do
            {
                x = random.Next(1, Width - 2);
                y = random.Next(1, Height - 2);
            } while (Grid[x, y] != CellType.Empty);

            return new Point(x, y);
        }
    }
}
