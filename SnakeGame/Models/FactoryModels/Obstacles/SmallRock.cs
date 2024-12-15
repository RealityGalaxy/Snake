using SnakeGame.Models.FactoryModels.Obstacles;

namespace SnakeGame.Models.FactoryModels
{
    public class SmallRock : ObstacleFlyweight
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
