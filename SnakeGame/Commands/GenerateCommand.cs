using SnakeGame.Factories;
using SnakeGame.Mementos;
using SnakeGame.Models;
using SnakeGame.Models.FactoryModels;
using SnakeGame.Services;
using SnakeGame.States;

namespace SnakeGame.Commands
{
    public class GenerateCommand : ICommand
    {
        public ILevelFactory LevelFactory { get; set; }
        public Map MapState { get; set; }
        public Dictionary<string, Snake> SnakeState { get; set; }
        public Dictionary<Point, Consumable> ConsumableState { get; set; }

        private GameMemento _backup;
        private int _level;

        public bool Execute(int instance, Dictionary<string, string> args)
        {
            GameInstance gameInstance = GameService.Instance.GameInstances[instance];
            _level = int.Parse(args["level"]);
            _backup = gameInstance.CreateMemento();

            switch (_level)
            {
                case 1:
                    gameInstance.LevelFactory = new Level1Factory();
                    LevelFactory = new Level1Factory();
                    break;
                case 2:
                    gameInstance.LevelFactory = new Level2Factory();
                    LevelFactory = new Level2Factory();
                    break;
                case 3:
                    gameInstance.LevelFactory = new Level3Factory();
                    LevelFactory = new Level3Factory();
                    break;
            }

            // Manually deep copy each dictionary
           /* MapState = gameInstance.Map; // Assuming Map has a copy constructor
            ConsumableState = gameInstance.Consumables.ToDictionary(
                entry => entry.Key,
                entry => entry.Value // Assuming Consumable has a copy constructor
            );
            SnakeState = gameInstance.Snakes.ToDictionary(
                entry => entry.Key,
                entry => entry.Value // Assuming Snake has a copy constructor
            );*/

            gameInstance.GenerateGame();
            return true;
        }


        public void Undo(int instance)
        {
            /*GameInstance gameInstance = GameService.Instance.GameInstances[instance];
            gameInstance.LevelFactory = LevelFactory;
            gameInstance.IsGameRunning = false;
            gameInstance.Map = MapState;
            gameInstance.Snakes = SnakeState;
            gameInstance.Consumables = ConsumableState;*/
            GameInstance gameInstance = GameService.Instance.GameInstances[instance];
            // Restore from the saved memento
            gameInstance.RestoreMemento(_backup);
        }
    }
}
