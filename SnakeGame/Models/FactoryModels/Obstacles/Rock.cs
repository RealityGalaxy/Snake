
using SnakeGame.Services;

namespace SnakeGame.Models.FactoryModels
{
    public class Rock : Obstacle
    {
        public Rock()
        {
            Points = new int[4, 4]
            {
                { 0, 1, 1, 0},
                { 1, 1, 1, 1},
                { 1, 1, 1, 1},
                { 0, 1, 1, 0},
            };
        }
    }
}
