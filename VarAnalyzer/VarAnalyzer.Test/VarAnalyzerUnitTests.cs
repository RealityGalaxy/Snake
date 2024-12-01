using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Xunit;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing.Verifiers;


public class VarDeclarationAnalyzerTests
{
    [Fact]
    public async Task DetectsVarUsage()
    {
        var testCode = @"
namespace TestNamespace
{
    class TestClass
    {
        void TestMethod()
        {
            var x = 5;
        }
    }
}";

        var expectedDiagnostic = DiagnosticResult
            .CompilerWarning("NoVarUsage")
            .WithSpan(7, 16, 7, 19)
            .WithMessage("Usage of 'var' keyword is discouraged");

        await VerifyAnalyzerAsync(testCode, expectedDiagnostic);
    }

    private static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
    {
        var test = new CSharpAnalyzerTest<VarDeclarationAnalyzer, XUnitVerifier>
        {
            TestCode = source,
        };

        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync();
    }
}
