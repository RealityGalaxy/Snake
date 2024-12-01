using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MethodLengthAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MethodLengthRule";

    private static readonly LocalizableString Title = "Method is too long";
    private static readonly LocalizableString MessageFormat = "Method '{0}' contains {1} lines, which exceeds the maximum allowed {2} lines.";
    private static readonly LocalizableString Description = "Methods should not be longer than 50 lines to maintain readability and manageability.";
    private const string Category = "Maintainability";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    private const int MaxMethodLines = 50;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        // Exclude auto-generated code
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // Register action to analyze method declarations
        context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
    }

    private static void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
    {
        var methodDeclaration = (MethodDeclarationSyntax)context.Node;

        // Get the syntax tree for the method
        var syntaxTree = methodDeclaration.SyntaxTree;
        var root = syntaxTree.GetRoot(context.CancellationToken);

        // Get the span of the method body
        var body = methodDeclaration.Body;
        var expressionBody = methodDeclaration.ExpressionBody;

        // If method has no body (e.g., interface method), skip analysis
        if (body == null && expressionBody == null)
        {
            return;
        }

        // Determine the start and end lines of the method
        FileLinePositionSpan methodSpan;

        if (body != null)
        {
            methodSpan = body.GetLocation().GetLineSpan();
        }
        else // Expression-bodied method (arrow expression)
        {
            methodSpan = expressionBody.GetLocation().GetLineSpan();
        }

        int startLine = methodSpan.StartLinePosition.Line;
        int endLine = methodSpan.EndLinePosition.Line;

        int lineCount = endLine - startLine + 1;

        if (lineCount > MaxMethodLines)
        {
            var methodName = methodDeclaration.Identifier.Text;

            var diagnostic = Diagnostic.Create(
                Rule,
                methodDeclaration.Identifier.GetLocation(),
                methodName,
                lineCount,
                MaxMethodLines);

            context.ReportDiagnostic(diagnostic);
        }
    }
}