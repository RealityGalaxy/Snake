namespace SnakeGame.Models.FactoryModels
{
    public abstract class Obstacle : IPrototype<Obstacle>
    {
        public int[,] Points { get; set; }

        public abstract Obstacle Clone();

        public bool CheckPlacement(Point position, Map map)
        {
            for (int i = 0; i < Points.GetLength(0); i++)
            {
                for (int j = 0; j < Points.GetLength(1); j++)
                {
                    var pos_x = j + position.X;
                    int pos_y = i + position.Y;
                    if (Points[i, j] == 1 && (pos_y >= map.Width || pos_x >= map.Height || map.Grid[pos_x, pos_y] != Map.CellType.Empty))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Place(Point position, Map map)
        {
            for (int i = 0; i < Points.GetLength(0); i++)
            {
                for (int j = 0; j < Points.GetLength(1); j++)
                {
                    var pos_x = j + position.X;
                    int pos_y = i + position.Y;
                    if (Points[i, j] == 1 && pos_y < map.Width && pos_x < map.Height)
                    {
                        map.Grid[pos_x, pos_y] = Map.CellType.Wall;
                    }
                }
            }
        }
    }
}
