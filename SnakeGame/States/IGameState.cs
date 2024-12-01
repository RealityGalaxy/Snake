using SnakeGame.Services;

namespace SnakeGame.States
{
    public interface IGameState
    {
        void EnterState(GameInstance instance); // This method is called when the state is entered

        // These methods are called when the game is started, stopped, paused, resumed, or ended
        void StartGame(GameInstance instance); 
        void PauseGame(GameInstance instance);
        void ResumeGame(GameInstance instance);
        void EndGame(GameInstance instance);
        string ToString(); // This method is used to get the name of the state
    }
}
