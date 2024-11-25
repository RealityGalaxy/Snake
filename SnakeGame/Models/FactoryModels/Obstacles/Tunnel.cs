using SnakeGame.Models.FactoryModels.Obstacles;

namespace SnakeGame.Models.FactoryModels
{
    public class Tunnel : ObstacleFlyweight
    {
        public Tunnel()
        {
            Random rand = new Random();
            if(rand.Next(2) == 0)
            {
                Points = new int[6, 4]
                {
                    { 1, 0, 0, 1},
                    { 1, 0, 0, 1},
                    { 1, 0, 0, 1},
                    { 1, 0, 0, 1},
                    { 1, 0, 0, 1},
                    { 1, 0, 0, 1},
                };
            }
            else
            {
                Points = new int[4, 6]
                {
                    { 1, 1, 1, 1, 1, 1},
                    { 0, 0, 0, 0, 0, 0},
                    { 0, 0, 0, 0, 0, 0},
                    { 1, 1, 1, 1, 1, 1},
                };
            }
        }
    }
}
