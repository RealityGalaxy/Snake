using SnakeGame.Services;

namespace SnakeGame.Commands
{
    public class ResumeCommand : ICommand
    {
        public bool Execute(int instance, Dictionary<string, string> args)
        {
            GameService.Instance.GameInstances[instance].ResumeGame();
            return false;
        }

        public void Undo(int instance) { }
    }
}
