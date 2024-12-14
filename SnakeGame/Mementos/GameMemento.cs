using SnakeGame.Factories;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Models;
using SnakeGame.Services;
using SnakeGame.Models.FactoryModels; // This gives access to the Map class and CellType.
using SnakeGame.Services;

namespace SnakeGame.Mementos
{
    public class GameMemento
    {
        public Map.CellType[,] MapState { get; private set; }
        public Dictionary<string, Snake> SnakeState { get; private set; }
        public Dictionary<Point, Consumable> ConsumableState { get; private set; }
        public ILevelFactory LevelFactoryState { get; private set; }



        public GameMemento(
            Map.CellType[,] map,
            Dictionary<string, Snake> snakes,
            Dictionary<Point, Consumable> consumables,
            ILevelFactory levelFactory)
        {
            // Deep copy for immutability
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            MapState = new Map.CellType[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    MapState[x, y] = map[x, y];
                }
            }

            SnakeState = snakes.ToDictionary(entry => entry.Key, entry => entry.Value /* assuming shallow copy is enough, or implement a deep copy if needed */);
            ConsumableState = consumables.ToDictionary(entry => entry.Key, entry => entry.Value /* same note about copying */);
            LevelFactoryState = levelFactory;
        }
    }
}
