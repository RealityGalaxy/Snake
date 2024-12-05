namespace SnakeGame.Interpreter
{
    public class Parser
    {
        public static IExpression Parse(Context context)
        {
            var expressions = new List<IExpression>();

            while (context.PeekToken() != null)
            {
                var token = context.NextToken();
                if (token == "MOVE")
                {
                    var direction = context.NextToken();
                    expressions.Add(new MoveExpression(direction));
                }
                else if (token == "TURN")
                {
                    var direction = context.NextToken();
                    expressions.Add(new TurnExpression(direction));
                }
                else if (token == "REPEAT")
                {
                    var times = int.Parse(context.NextToken());
                    if (context.NextToken() != "[")
                        throw new Exception("Expected '[' after REPEAT command.");

                    var repeatExpressions = new List<IExpression>();
                    while (context.PeekToken() != "]")
                    {
                        repeatExpressions.Add(Parse(context));
                    }
                    context.NextToken(); // Consume ']'

                    expressions.Add(new RepeatExpression(times, repeatExpressions));
                }
                else
                {
                    throw new Exception($"Unknown command: {token}");
                }
            }

            return new SequenceExpression(expressions);
        }
    }

    public class SequenceExpression : IExpression
    {
        private List<IExpression> _expressions;

        public SequenceExpression(List<IExpression> expressions)
        {
            _expressions = expressions;
        }

        public void Interpret(Context context)
        {
            foreach (var expression in _expressions)
            {
                expression.Interpret(context);
            }
        }
    }
}
