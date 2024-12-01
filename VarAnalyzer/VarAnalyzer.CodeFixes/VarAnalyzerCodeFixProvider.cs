using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SpacingCodeFixProvider)), Shared]
public class SpacingCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(SpacingAnalyzer.DiagnosticId); }
    }

    public sealed override FixAllProvider GetFixAllProvider()
    {
        return WellKnownFixAllProviders.BatchFixer;
    }

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics[0];
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        var node = root.FindNode(diagnosticSpan);

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Add blank lines before and after",
                createChangedDocument: c => AddBlankLinesAsync(context.Document, node, c),
                equivalenceKey: "AddBlankLines"),
            diagnostic);
    }

    private async Task<Document> AddBlankLinesAsync(Document document, SyntaxNode node, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        var leadingTrivia = node.GetLeadingTrivia();
        var trailingTrivia = node.GetTrailingTrivia();

        var newLeadingTrivia = leadingTrivia;
        var newTrailingTrivia = trailingTrivia;

        // Add blank line before if missing
        if (!HasBlankLineBefore(leadingTrivia))
        {
            newLeadingTrivia = leadingTrivia.Insert(0, SyntaxFactory.EndOfLine("\n"));
        }

        // Add blank line after if missing
        if (!HasBlankLineAfter(trailingTrivia))
        {
            newTrailingTrivia = trailingTrivia.Add(SyntaxFactory.EndOfLine("\n"));
        }

        var newNode = node.WithLeadingTrivia(newLeadingTrivia).WithTrailingTrivia(newTrailingTrivia);

        var newRoot = root.ReplaceNode(node, newNode);

        return document.WithSyntaxRoot(newRoot);
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
            else if (trivia.IsKind(SyntaxKind.WhitespaceTrivia))
            {
                continue;
            }
            else
            {
                break; // Stop at any other trivia
            }
        }

        return newLineCount >= 2;
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
            else if (trivia.IsKind(SyntaxKind.WhitespaceTrivia))
            {
                continue;
            }
            else
            {
                break; // Stop at any other trivia
            }
        }

        return newLineCount >= 2;
    }
}
