using SnakeGame.Services;

namespace SnakeGame.Models.FactoryModels
{
    public class Level1Map : Map
    {
        public Level1Map(GameInstance instance) : base(20, 20, instance)
        {
            GenerateObstacles(3);
            Instance = instance;
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
                x = random.Next(1, Width - 2);
                y = random.Next(1, Height - 2);
            } while (Grid[x, y] != CellType.Empty);

            return new Point(x, y);
        }
    }
}
