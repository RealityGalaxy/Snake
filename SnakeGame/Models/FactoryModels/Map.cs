using SnakeGame.Models.FactoryModels.Maps.MapSizes;
using SnakeGame.Services;

namespace SnakeGame.Models.FactoryModels
{
    public abstract class Map
    {
        public GameInstance Instance { get; set; }
        public MapSize Size { get; set; }
        public CellType[,] Grid { get; private set; }

        public Map(GameInstance instance, MapSize size)
        {
            Instance = instance;
            Size = size;
            Grid = new CellType[size.Width, size.Height];
            InitializeGrid();
        }

        public void InitializeGrid()
        {
            // Initialize the grid with empty cells and walls
            for (int x = 0; x < Size.Width; x++)
            {
                for (int y = 0; y < Size.Height; y++)
                {
                    Grid[x, y] = CellType.Empty;
                }
            }

            // Add walls
            GenerateWalls();
        }

        private void GenerateWalls()
        {
            for (int x = 0; x < Size.Width; x++)
            {
                Grid[x, 0] = CellType.Wall;
                Grid[x, Size.Height - 1] = CellType.Wall;
            }
            for (int y = 0; y < Size.Height; y++)
            {
                Grid[0, y] = CellType.Wall;
                Grid[Size.Width - 1, y] = CellType.Wall;
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
