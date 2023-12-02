using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = DarkLink.Roslyn.CodeQuality.Test.CSharpAnalyzerVerifier<
    DarkLink.Roslyn.CodeQuality.DarkLinkRoslynCodeQualityAnalyzer>;

namespace DarkLink.Roslyn.CodeQuality.Test;

[TestClass]
public class DarkLinkRoslynCodeQualityUnitTest
{
    [TestMethod]
    public async Task WithEmptySource_NoDiagnostics()
    {
        var test = @"";

        await VerifyCS.VerifyAnalyzerAsync(test);
    }
}
