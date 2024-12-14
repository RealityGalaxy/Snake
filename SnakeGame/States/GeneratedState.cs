using SnakeGame.Services;

namespace SnakeGame.States
{
    public class GeneratedState : IGameState
    {
        public void EnterState(GameInstance instance)
        {
            Console.WriteLine("GeneratedState log: Generating game...");
            instance.ResetGame();
            Console.WriteLine("GeneratedState log: Game generated.");
        }

        public void StartGame(GameInstance instance)
        {
            Console.WriteLine("GeneratedState log: Game starting...");
            instance.SetState(new StartedState());
        }

        public void PauseGame(GameInstance instance) => Console.WriteLine("GeneratedState log: Cannot pause game. Game is not started yet.");
        public void ResumeGame(GameInstance instance) => Console.WriteLine("GeneratedState log: Cannot resume game. Game is not started yet.");
        public void EndGame(GameInstance instance) => Console.WriteLine("GeneratedState log: Cannot end game. Game is not started yet.");

        public string ToString() => "Generated";
    }
}
