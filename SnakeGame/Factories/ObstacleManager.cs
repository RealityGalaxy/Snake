using SnakeGame.Models.FactoryModels;
using SnakeGame.Models.FactoryModels.Obstacles;

namespace SnakeGame.Factories
{
    public class ObstacleManager
    {
        public static Dictionary<string, ObstacleFlyweight> obstacles = [];

        public static ObstacleFlyweight GetObstacleFlyweight(string name)
        {
            if (!obstacles.ContainsKey(name))
            {
                switch (name)
                {
                    case "rock":
                        obstacles[name] = new Rock();
                        break;
                    case "small_rock":
                        obstacles[name] = new SmallRock();
                        break;
                    case "tunnel":
                        obstacles[name] = new Tunnel();
                        break;
                    case "small_tunnel":
                        obstacles[name] = new SmallTunnel();
                        break;
                    case "room":
                        obstacles[name] = new Room();
                        break;
                }
            }
            return obstacles[name];
        }
    }
}
