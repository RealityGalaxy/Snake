using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace SnakeGameAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MyAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "SN0001";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Var wrong",
            "Try using not var",
            "Design",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // Example of registering a syntax node action
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, Microsoft.CodeAnalysis.CSharp.SyntaxKind.LocalDeclarationStatement);
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            // Example logic: Report a diagnostic for "var" usage
            var declaration = (Microsoft.CodeAnalysis.CSharp.Syntax.LocalDeclarationStatementSyntax)context.Node;

            if (declaration.Declaration.Type.IsVar)
            {
                var diagnostic = Diagnostic.Create(Rule, declaration.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
