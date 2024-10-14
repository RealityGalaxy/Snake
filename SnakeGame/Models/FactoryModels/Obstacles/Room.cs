
using SnakeGame.Services;

namespace SnakeGame.Models.FactoryModels
{
    public class Room : Obstacle
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
