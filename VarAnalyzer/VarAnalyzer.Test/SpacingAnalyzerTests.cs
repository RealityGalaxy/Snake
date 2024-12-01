using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using Microsoft;
using Microsoft.CodeAnalysis.Testing.Verifiers;
public class SpacingAnalyzerTests
{
    [Fact]
    public async Task IfStatement_MissingBlankLines_ReportsDiagnostic()
    {
        var testCode = @"
namespace TestNamespace
{
    class TestClass
    {
        void TestMethod()
        {
            int x = 5;
            if (x > 0)
            {
                x++;
            }
            int y = 10;
        }
    }
}"
        ;

        var expected = DiagnosticResult.CompilerWarning("SpacingRule")
            .WithSpan(8, 16, 10, 17)
        .WithArguments("if");

        await VerifyAnalyzerAsync(testCode, expected);
    }

    [Fact]
    public async Task IfStatement_WithBlankLines_NoDiagnostic()
    {
        var testCode = @"
namespace TestNamespace
{
    class TestClass
    {
        void TestMethod()
        {
            int x = 5;

            if (x > 0)
            {
                x++;
            }

            int y = 10;
        }
    }
}"
        ;

        await VerifyAnalyzerAsync(testCode);
    }

    private static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
    {
        var test = new CSharpAnalyzerTest<SpacingAnalyzer, XUnitVerifier>
        {
            TestCode = source,
        };

        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync();
    }
}
