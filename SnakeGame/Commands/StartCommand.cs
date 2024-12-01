using SnakeGame.Services;

namespace SnakeGame.Commands
{
    public class StartCommand : ICommand
    {
        public bool Execute(int instance, Dictionary<string, string> args)
        {
            GameService.Instance.GameInstances[instance].StartGame();
            return false;
        }

        public void Undo(int instance) { }
    }
}
