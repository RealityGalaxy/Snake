namespace SnakeGame.Models.FactoryModels
{
    public abstract class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public CellType[,] Grid { get; private set; }

        public Map(int height, int width)
        {
            Height = height;
            Width = width;
            Grid = new CellType[Width, Height];
            InitializeGrid();
        }

        public void InitializeGrid()
        {
            // Initialize the grid with empty cells and walls
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Grid[x, y] = CellType.Empty;
                }
            }

            // Add walls
            GenerateWalls();
        }

        private void GenerateWalls()
        {
            for (int x = 0; x < Width; x++)
            {
                Grid[x, 0] = CellType.Wall;
                Grid[x, Height - 1] = CellType.Wall;
            }
            for (int y = 0; y < Height; y++)
            {
                Grid[0, y] = CellType.Wall;
                Grid[Width - 1, y] = CellType.Wall;
            }
        }

        public enum CellType
        {
            Empty,
            Wall,
            Snake,
            Consumable
        }
    }
}
