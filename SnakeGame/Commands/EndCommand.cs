using SnakeGame.Services;

namespace SnakeGame.Commands
{
    public class EndCommand : ICommand
    {
        public bool Execute(int instance, Dictionary<string, string> args)
        {
            GameService.Instance.GameInstances[instance].EndGame();
            return false;
        }

        public void Undo(int instance) { }
    }
}
