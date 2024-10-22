using SnakeGame.Models.FactoryModels;
using SnakeGame.Services;

namespace SnakeGame.Factories
{
    public interface ILevelFactory
    {
        public Consumable generateConsumable(GameInstance instance);
        public Map generateMap(GameInstance instance);
        public Obstacle generateObstacle();
    }
}
