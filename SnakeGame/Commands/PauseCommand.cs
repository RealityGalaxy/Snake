using SnakeGame.Services;

namespace SnakeGame.Commands
{
    public class PauseCommand : ICommand
    {
        public bool Execute(int instance, Dictionary<string, string> args)
        {
            GameService.Instance.GameInstances[instance].IsGameRunning = false;
            return false;
        }

        public void Undo(int instance) { }
    }
}
