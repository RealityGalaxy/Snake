using SnakeGame.Services;

namespace SnakeGame.States
{
    public class StartedState : IGameState
    {
        public void EnterState(GameInstance instance)
        {
            Console.WriteLine("StartedState log: Game starting...");
            instance.IsGameRunning = true;
            instance.StartTimer();
            Console.WriteLine("StartedState log: Game started.");
        }

        public void StartGame(GameInstance instance) => Console.WriteLine("StartedState log: Cannot start game. Game is already started.");

        public void PauseGame(GameInstance instance)
        {
            Console.WriteLine("StartedState log: Game pausing...");
            instance.SetState(new StoppedState());
        }

        public void ResumeGame(GameInstance instance) => Console.WriteLine("StartedState log: Cannot resume game. Game is already started.");

        public void EndGame(GameInstance instance)
        {
            Console.WriteLine("StartedState log: Game ending...");
            instance.SetState(new EndState());
        }

        public string ToString() => "Started";
    }
}
