using SnakeGame.Services;

namespace SnakeGame.Models.FactoryModels
{
    public class Level3Map : Map
    {
        public Level3Map() : base(40, 40)
        {
            GenerateObstacles(5);
        }

        public void GenerateObstacles(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Obstacle obstacle = GameService.Instance.LevelFactory.generateObstacle();
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
                x = random.Next(1, Width - 2);
                y = random.Next(1, Height - 2);
            } while (Grid[x, y] != CellType.Empty);

            return new Point(x, y);
        }
    }
}
