using SnakeGame.Models.FactoryModels;
using SnakeGame.Models;
using SnakeGame.Services;

namespace SnakeGame.States
{
    public class EndState : IGameState
    {
        public void EnterState(GameInstance instance)
        {
            instance.Snakes.Clear();
            instance.Consumables.Clear();
            instance.IsGameRunning = false;
            Console.WriteLine("EndState log: Game ended.");
        }
        public void StartGame(GameInstance instance) => Console.WriteLine("EndState log: Cannot start game. Game is already ended.");

        public void PauseGame(GameInstance instance) => Console.WriteLine("EndState log: Cannot pause game. Game is already ended.");

        public void ResumeGame(GameInstance instance) => Console.WriteLine("EndState log: Cannot resume game. Game is already ended.");

        public void EndGame(GameInstance instance) => Console.WriteLine("EndState log: Cannot end game. Game is already ended.");

        public string ToString() => "Ended";
    }
}
