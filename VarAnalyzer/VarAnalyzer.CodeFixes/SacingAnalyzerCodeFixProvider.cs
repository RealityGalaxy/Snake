using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(VarDeclarationCodeFixProvider)), Shared]
public class VarDeclarationCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds
    {
        get { return ImmutableArray.Create(VarDeclarationAnalyzer.DiagnosticId); }
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

        var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<VariableDeclarationSyntax>().First();

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Use explicit type",
                createChangedDocument: c => ReplaceVarWithExplicitTypeAsync(context.Document, declaration, c),
                equivalenceKey: "Use explicit type"),
            diagnostic);
    }

    private async Task<Document> ReplaceVarWithExplicitTypeAsync(Document document, VariableDeclarationSyntax declaration, CancellationToken cancellationToken)
    {
        var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

        var typeInfo = semanticModel.GetTypeInfo(declaration.Type, cancellationToken);
        var explicitType = SyntaxFactory.ParseTypeName(typeInfo.ConvertedType.ToDisplayString());

        var newDeclaration = declaration.WithType(explicitType);

        var root = await document.GetSyntaxRootAsync(cancellationToken);
        var newRoot = root.ReplaceNode(declaration, newDeclaration);

        return document.WithSyntaxRoot(newRoot);
    }
}
