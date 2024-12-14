namespace SnakeGame.Interpreter
{
    public class RepeatExpression : IExpression
    {
        private int _times;
        private List<IExpression> _expressions;

        public RepeatExpression(int times, List<IExpression> expressions)
        {
            _times = times;
            _expressions = expressions;
        }

        public void Interpret(Context context)
        {
            for (int i = 0; i < _times; i++)
            {
                foreach (var expression in _expressions)
                {
                    expression.Interpret(context);
                }
            }
        }
    }
}
