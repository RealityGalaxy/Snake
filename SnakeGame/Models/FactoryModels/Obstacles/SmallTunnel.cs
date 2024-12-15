using SnakeGame.Models.FactoryModels.Obstacles;

namespace SnakeGame.Models.FactoryModels
{
    public class SmallTunnel : ObstacleFlyweight
    {
        public SmallTunnel()
        {
            Random rand = new Random();
            if(rand.Next(2) == 0)
            {
                Points = new int [4, 4]
                {
                    { 1, 0, 0, 1},
                    { 1, 0, 0, 1},
                    { 1, 0, 0, 1},
                    { 1, 0, 0, 1},
                };
            }
            else
            {
                Points = new int[4, 4]
                {
                    { 1, 1, 1, 1},
                    { 0, 0, 0, 0},
                    { 0, 0, 0, 0},
                    { 1, 1, 1, 1},
                };
            }
        }
    }
}
