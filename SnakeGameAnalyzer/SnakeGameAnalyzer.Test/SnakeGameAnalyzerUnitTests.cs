// File: MyAnalyzerAnalyzerTests.cs
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace SnakeGameAnalyzer.Test
{
    [TestClass]
    public class MyAnalyzerAnalyzerTests
    {
        private const string DiagnosticId = "VAR001";

        // Test case where 'var' is used - should trigger a diagnostic
        [TestMethod]
        public async Task VarUsage_ShouldTriggerDiagnostic()
        {
            var testCode = @"using System;

namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod()
        {
            var number = 5;
        }
    }
}";

            var expected = CSharpAnalyzerVerifier<MyAnalyzerAnalyzer>.Diagnostic(DiagnosticId)
                .WithSpan(9, 13, 9, 16); // Adjust the span based on your code structure

            await CSharpAnalyzerVerifier<MyAnalyzerAnalyzer>.VerifyAnalyzerAsync(testCode, expected);
        }

        // Test case where explicit type is used - should not trigger any diagnostic
        [TestMethod]
        public async Task ExplicitTypeUsage_ShouldNotTriggerDiagnostic()
        {
            var testCode = @"
using System;

namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod()
        {
            int number = 5;
        }
    }
}";

            await CSharpAnalyzerVerifier<MyAnalyzerAnalyzer>.VerifyAnalyzerAsync(testCode);
        }

        // Additional test cases can be added here
    }
}
