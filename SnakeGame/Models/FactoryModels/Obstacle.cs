using SnakeGame.Factories;
using SnakeGame.Iterators;

namespace SnakeGame.Models.FactoryModels
{
    public class Obstacle
    {
        public string Name { get; set; }

        public Obstacle(string name)
        {
            Name = name;
        }

        public bool CheckPlacement(Point position, Map map)
        {
            var iterator = GetIterator();

            while (iterator.HasNext())
            {
                var point = iterator.Next();

                var pos_x = point.X + position.X;
                var pos_y = point.Y + position.Y;

                if (pos_x >= map.Size.Width || pos_y >= map.Size.Height || map.Grid[pos_x, pos_y] != Map.CellType.Empty)
                {
                    return false;
                }
            }
            return true;
        }

        public void Place(Point position, Map map)
        {
            var iterator = GetIterator();

            while (iterator.HasNext())
            {
                var point = iterator.Next();

                var pos_x = point.X + position.X;
                var pos_y = point.Y + position.Y;

                if (pos_y < map.Size.Width && pos_x < map.Size.Height)
                {
                    map.Grid[pos_x, pos_y] = Map.CellType.Wall;
                }
            }
        }

        public IIterator<Point> GetIterator()
        {
            return new ObstaclePointIterator(ObstacleManager.GetObstacleFlyweight(Name).Points);
        }
    }
}
