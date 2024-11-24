using SnakeGame.Services;

namespace SnakeGame.Commands
{
    public class JoinCommand : ICommand
    {
        public string ConnectionId { get; set; }
        public bool Execute(int instance, Dictionary<string, string> args)
        {
            ConnectionId = args["connectionId"];
            GameService.Instance.GameInstances[instance].AddSnake(args["connectionId"], args["color"], args["name"], instance, bool.Parse(args["manual"]));
            return true;
        }

        public void Undo(int instance)
        {
            GameService.Instance.GameInstances[instance].RemoveSnake(ConnectionId);
        }
    }
}
