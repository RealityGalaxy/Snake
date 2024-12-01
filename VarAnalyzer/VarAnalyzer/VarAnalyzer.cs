using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class VarDeclarationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "NoVarUsage";

    private static readonly LocalizableString Title = "Avoid using 'var' keyword";
    private static readonly LocalizableString MessageFormat = "Usage of 'var' keyword is discouraged";
    private static readonly LocalizableString Description = "Explicitly type your variable declarations instead of using 'var'";
    private const string Category = "Style";

    private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

    public override void Initialize(AnalysisContext context)
    {
        // To optimize performance, configure analysis settings
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // Register an action to be called for each VariableDeclarationSyntax node
        context.RegisterSyntaxNodeAction(AnalyzeVariableDeclaration, SyntaxKind.VariableDeclaration);
    }

    private static void AnalyzeVariableDeclaration(SyntaxNodeAnalysisContext context)
    {
        var variableDeclaration = (VariableDeclarationSyntax)context.Node;

        // Check if the variable is implicitly typed (using 'var')
        if (variableDeclaration.Type.IsVar)
        {
            // Report a diagnostic at the location of the 'var' keyword
            var diagnostic = Diagnostic.Create(Rule, variableDeclaration.Type.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }
}
