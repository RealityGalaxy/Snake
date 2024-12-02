using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NoHardcodedValuesAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "NoHardcodedValuesRule";

    private static readonly LocalizableString Title = "Avoid using hardcoded values in method calls";
    private static readonly LocalizableString MessageFormat = "Method '{0}' is called with a hardcoded value as parameter";
    private static readonly LocalizableString Description = "Methods should not be called with hardcoded (literal) values as parameters to improve code maintainability.";
    private const string Category = "BestPractices";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        // Exclude auto-generated code
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // Register action to analyze invocation expressions
        context.RegisterSyntaxNodeAction(AnalyzeInvocationExpression, SyntaxKind.InvocationExpression);
    }

    private static void AnalyzeInvocationExpression(SyntaxNodeAnalysisContext context)
    {
        var invocationExpression = (InvocationExpressionSyntax)context.Node;

        var argumentList = invocationExpression.ArgumentList;
        if (argumentList == null)
        {
            return;
        }

        foreach (var argument in argumentList.Arguments)
        {
            var expression = argument.Expression;

            if (expression is LiteralExpressionSyntax)
            {
                var symbolInfo = context.SemanticModel.GetSymbolInfo(invocationExpression, context.CancellationToken);
                var methodSymbol = symbolInfo.Symbol as IMethodSymbol;

                if (methodSymbol != null)
                {
                    var methodName = methodSymbol.Name;

                    var diagnostic = Diagnostic.Create(
                        Rule,
                        expression.GetLocation(),
                        methodName);

                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}