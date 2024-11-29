using SnakeGame.Services;

namespace SnakeGame.States
{
    public class StoppedState : IGameState
    {
        public void EnterState(GameInstance instance)
        {
            instance.IsGameRunning = false;
            instance.PauseTimer();
            Console.WriteLine("StoppedState log: Game stopped.");
        }

        public void StartGame(GameInstance instance) => Console.WriteLine("StoppedState log: Cannot start. Game paused.");

        public void PauseGame(GameInstance instance) => Console.WriteLine("StoppedState log: Cannot pause. Game is already paused.");

        public void ResumeGame(GameInstance instance)
        {
            Console.WriteLine("StoppedState log: Game resuming...");
            instance.SetState(new StartedState());
        }

        public void EndGame(GameInstance instance)
        {
            Console.WriteLine("StoppedState log: Game ending...");
            instance.SetState(new EndState());
        }

        public string ToString() => "Stopped";
    }
}
