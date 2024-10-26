namespace SnakeGame.Commands
{
    public interface ICommand
    {
        public bool Execute(int instance, Dictionary<string, string> args);
        public void Undo(int instance);
    }
}
