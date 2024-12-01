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

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            "VAR001",
            "Avoid 'var' for type declarations",
            "'var' should not be used; use explicit type declarations.",
            "CodeStyle",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);


        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeVariableDeclaration, SyntaxKind.VariableDeclaration);
        }

        private void AnalyzeVariableDeclaration(SyntaxNodeAnalysisContext context)
        {
            var variableDeclaration = (VariableDeclarationSyntax)context.Node;

            // Check if the type is 'var'
            if (variableDeclaration.Type.IsVar)
            {
                // Report a diagnostic
                var diagnostic = Diagnostic.Create(Rule, variableDeclaration.Type.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }

    }
}
