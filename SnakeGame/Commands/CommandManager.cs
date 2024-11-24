namespace SnakeGame.Commands
{
    public class CommandManager
    {
        public Stack<ICommand> History { get; set; } = new Stack<ICommand>();
        public static PauseCommand Pause { get { return new PauseCommand(); } }
        public static GenerateCommand Generate { get { return new GenerateCommand(); } }
        public static StartCommand Start { get { return new StartCommand(); } }
        public static JoinCommand Join { get { return new JoinCommand(); } }

        public CommandManager() { }

        public void ExecuteCommand(ICommand command, int instance, Dictionary<string, string> args = null)
        {
            if (command.Execute(instance, args))
            {
                History.Push(command);
            }
        }
        public void Undo(int instance)
        {
            var Command = History.Peek();
            Command.Undo(instance);
            History.Pop();
            return;
        }
    }
}
