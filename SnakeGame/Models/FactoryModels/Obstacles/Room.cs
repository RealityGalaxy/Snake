using SnakeGame.Models.FactoryModels.Obstacles;

namespace SnakeGame.Models.FactoryModels
{
    public class Room : ObstacleFlyweight
    {
        public Room()
        {
            Points = new int[8,8]
            {
                { 1, 1, 1, 0, 0, 1, 1, 1},
                { 1, 0, 0, 0, 0, 0, 0, 1},
                { 1, 0, 1, 1, 0, 1, 0, 1},
                { 0, 0, 0, 0, 0, 1, 0, 0},
                { 0, 0, 1, 0, 0, 0, 0, 0},
                { 1, 0, 1, 0, 1, 1, 0, 1},
                { 1, 0, 0, 0, 0, 0, 0, 1},
                { 1, 1, 1, 0, 0, 1, 1, 1},
            };
        }
    }
}
