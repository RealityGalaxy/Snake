using SnakeGame.Models.FactoryModels.Maps.MapSizes;
using SnakeGame.Services;

namespace SnakeGame.Models.FactoryModels
{
    public class Level3Map : Map
    {
        public Level3Map(GameInstance instance, MapSize size) : base(instance, size)
        {
            GenerateInnerWalls();
            GenerateObstacles(10);
        }
        public void GenerateInnerWalls()
        {
            for (int i = 15; i < 25; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Grid[i, j] = CellType.Wall;
                }
            }

            for (int i = 15; i < 25; i++)
            {
                for (int j = 30; j < 40; j++)
                {
                    Grid[i, j] = CellType.Wall;
                }
            }

            for (int j = 15; j < 25; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    Grid[i, j] = CellType.Wall;
                }
            }

            for (int j = 15; j < 25; j++)
            {
                for (int i = 30; i < 40; i++)
                {
                    Grid[i, j] = CellType.Wall;
                }
            }


        }

        public void GenerateObstacles(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Obstacle obstacle = Instance.LevelFactory.generateObstacle();
                Point position = GetRandomEmptyPosition();
                while (!obstacle.CheckPlacement(position, this))
                {
                    position = GetRandomEmptyPosition();
                }
                obstacle.Place(position, this);
            }
        }

        public Point GetRandomEmptyPosition()
        {
            var random = new Random();
            int x, y;

            do
            {
                x = random.Next(1, Size.Width - 2);
                y = random.Next(1, Size.Height - 2);
            } while (Grid[x, y] != CellType.Empty);

            return new Point(x, y);
        }
    }
}
