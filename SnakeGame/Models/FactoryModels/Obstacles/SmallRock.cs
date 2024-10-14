
using SnakeGame.Services;

namespace SnakeGame.Models.FactoryModels
{
    public class SmallRock : Obstacle
    {
        public SmallRock()
        {
            Points = new int[2, 2]
            {
                { 1, 1},
                { 1, 1},
            };
        }
    }
}
