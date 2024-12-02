using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class SpacingAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SpacingRule";

    private static readonly LocalizableString Title = "Missing blank lines before or after statement";
    private static readonly LocalizableString MessageFormat = "Add blank lines before and after this '{0}' statement";
    private static readonly LocalizableString Description = "Statements should be surrounded by blank lines for better readability.";
    private const string Category = "Formatting";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.IfStatement, SyntaxKind.ForStatement, SyntaxKind.WhileStatement);
    }

    private void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var statementNode = context.Node;
        var syntaxTree = statementNode.SyntaxTree;


        var leadingTrivia = statementNode.GetLeadingTrivia();
        var trailingTrivia = statementNode.GetTrailingTrivia();

        bool hasBlankLineBefore = HasBlankLineBefore(leadingTrivia);
        bool hasBlankLineAfter = HasBlankLineAfter(trailingTrivia);

        if (!hasBlankLineBefore || !hasBlankLineAfter)
        {
            var statementKind = statementNode.Kind().ToString().Replace("Statement", "").ToLower();
            string message;

            if (!hasBlankLineBefore && !hasBlankLineAfter)
            {
                message = $"Add blank lines before and after this '{statementKind}' statement";
            }
            else if (!hasBlankLineBefore)
            {
                message = $"Add a blank line before this '{statementKind}' statement";
            }
            else
            {
                message = $"Add a blank line after this '{statementKind}' statement";
            }

            var diagnostic = Diagnostic.Create(
                new DiagnosticDescriptor(
                    DiagnosticId,
                    Title,
                    message,
                    Category,
                    DiagnosticSeverity.Warning,
                    isEnabledByDefault: true,
                    description: Description),
                statementNode.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }


    private bool HasBlankLineBefore(SyntaxTriviaList leadingTrivia)
    {
        int newLineCount = 0;

        for (int i = leadingTrivia.Count - 1; i >= 0; i--)
        {
            var trivia = leadingTrivia[i];

            if (trivia.IsKind(SyntaxKind.EndOfLineTrivia))
            {
                newLineCount++;
            }
            else if (trivia.IsKind(SyntaxKind.WhitespaceTrivia) || trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) || trivia.IsKind(SyntaxKind.MultiLineCommentTrivia))
            {
                continue;
            }
            else
            {
                break;
            }

            if (newLineCount >= 1)
            {
                return true;
            }
        }

        return newLineCount >= 1; 
    }


    private bool HasBlankLineAfter(SyntaxTriviaList trailingTrivia)
    {
        int newLineCount = 0;

        for (int i = 0; i < trailingTrivia.Count; i++)
        {
            var trivia = trailingTrivia[i];

            if (trivia.IsKind(SyntaxKind.EndOfLineTrivia))
            {
                newLineCount++;
            }
            else if (trivia.IsKind(SyntaxKind.WhitespaceTrivia) || trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) || trivia.IsKind(SyntaxKind.MultiLineCommentTrivia))
            {
                continue;
            }
            else
            {
                break;
            }

            if (newLineCount >= 1)
            {
                return true;
            }
        }

        return newLineCount >= 1;
    }


}
