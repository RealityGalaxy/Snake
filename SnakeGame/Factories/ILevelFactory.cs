using SnakeGame.Models.FactoryModels;

namespace SnakeGame.Factories
{
    public interface ILevelFactory
    {
        public Consumable generateConsumable();
        public Map generateMap();
        public Obstacle generateObstacle();
    }
}
