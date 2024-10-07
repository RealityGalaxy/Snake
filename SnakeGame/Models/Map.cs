namespace SnakeGame.Models
{
    public class Map
    {
        private static readonly Lazy<Map> _instance = new(() => new Map());
        public static Map Instance => _instance.Value;

        public int Width { get; } = 30;
        public int Height { get; } = 30;
        public CellType[,] Grid { get; private set; }

        private Map()
        {
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

            // Add walls (for example, the borders)
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
