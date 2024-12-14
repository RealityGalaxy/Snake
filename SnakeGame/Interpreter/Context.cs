using SnakeGame.Models;

namespace SnakeGame.Interpreter
{
    public class Context
    {
        public Snake Snake { get; set; }
        public Queue<string> Tokens { get; private set; }

        public Context(Snake snake, string command)
        {
            Snake = snake;
            Tokens = new Queue<string>(command.Split(' '));
        }

        public string NextToken()
        {
            return Tokens.Count > 0 ? Tokens.Dequeue() : null;
        }

        public string PeekToken()
        {
            return Tokens.Count > 0 ? Tokens.Peek() : null;
        }
    }
}
