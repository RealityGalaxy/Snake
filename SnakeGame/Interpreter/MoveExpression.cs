using SnakeGame.Models;

namespace SnakeGame.Interpreter
{
    public class MoveExpression : IExpression
    {
        private string _direction;

        public MoveExpression(string direction)
        {
            _direction = direction;
        }

        public void Interpret(Context context)
        {
            // Update the snake's direction
            if (context.Snake.IsAlive)
            {
                context.Snake.Turn(Enum.Parse<Snake.Direction>(_direction, true));
            }
        }
    }

    public class TurnExpression : IExpression
    {
        private string _direction;

        public TurnExpression(string direction)
        {
            _direction = direction;
        }

        public void Interpret(Context context)
        {
            // Update the snake's direction
            if (context.Snake.IsAlive)
            {
                context.Snake.Turn(Enum.Parse<Snake.Direction>(_direction, true));
            }
        }
    }
}
