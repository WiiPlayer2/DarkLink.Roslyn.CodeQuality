using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = DarkLink.Roslyn.CodeQuality.Test.CSharpAnalyzerVerifier<
    DarkLink.Roslyn.CodeQuality.DarkLinkRoslynCodeQualityAnalyzer>;

namespace DarkLink.Roslyn.CodeQuality.Test;

[TestClass]
public class DarkLinkRoslynCodeQualityUnitTest
{
    [TestMethod]
    public async Task WithCompositionInLogic_RaisesWarnings()
    {
        var test = @"
class P
{
    private static int L(int arg1, bool arg2) {
        if (arg1 == 0) return -1;
        if (arg2) return -2;
        return C();
    }

    public static int C() => 0;
}
";

        await VerifyCS.VerifyAnalyzerAsync(
            test,
            DiagnosticResult.CompilerWarning(Diagnostics.CQ0001_MethodMixed.Id).WithLocation(4, 24),
            DiagnosticResult.CompilerWarning(Diagnostics.CQ0003_CompositionInLogic.Id).WithLocation(7, 16));
    }

    [TestMethod]
    public async Task WithEmptySource_NoDiagnostics()
    {
        var test = @"";

        await VerifyCS.VerifyAnalyzerAsync(test);
    }

    [TestMethod]
    public async Task WithForeachLoopInComposition_RaisesWarnings()
    {
        var test = @"
class P
{
    private static int L(bool s) => s ? 0 : 1; 

    public static void C()
    {
        var v1 = L(true);
        var v2 = L(false);
        
        foreach(var i in new[]{1, 2, 3})
        {
            L(false);
        }
    }
}
";

        await VerifyCS.VerifyAnalyzerAsync(
            test,
            DiagnosticResult.CompilerWarning(Diagnostics.CQ0001_MethodMixed.Id).WithLocation(6, 24),
            DiagnosticResult.CompilerWarning(Diagnostics.CQ0002_LogicInComposition.Id).WithLocation(11, 9));
    }

    [TestMethod]
    public async Task WithForLoopInComposition_RaisesWarnings()
    {
        var test = @"
class P
{
    private static int L(bool s) => s ? 0 : 1; 

    public static void C()
    {
        var v1 = L(true);
        var v2 = L(false);
        
        for(var i = 0; i < 10; i++)
        {
            L(false);
        }
    }
}
";

        await VerifyCS.VerifyAnalyzerAsync(
            test,
            DiagnosticResult.CompilerWarning(Diagnostics.CQ0001_MethodMixed.Id).WithLocation(6, 24),
            DiagnosticResult.CompilerWarning(Diagnostics.CQ0002_LogicInComposition.Id).WithLocation(11, 9));
    }

    [TestMethod]
    public async Task WithLogicInComposition_RaisesWarnings()
    {
        var test = @"
class P
{
    private static int L(bool s) => s ? 0 : 1; 

    public static int C()
    {
        var v1 = L(true);
        var v2 = L(false);
        return v1 > v2 ? 0 : 1;
    }
}
";

        await VerifyCS.VerifyAnalyzerAsync(
            test,
            DiagnosticResult.CompilerWarning(Diagnostics.CQ0001_MethodMixed.Id).WithLocation(6, 23),
            DiagnosticResult.CompilerWarning(Diagnostics.CQ0002_LogicInComposition.Id).WithLocation(10, 16));
    }
}
