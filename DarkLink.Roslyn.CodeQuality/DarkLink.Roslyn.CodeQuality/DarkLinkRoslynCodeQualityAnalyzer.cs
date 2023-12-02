using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DarkLink.Roslyn.CodeQuality;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DarkLinkRoslynCodeQualityAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        Diagnostics.CQ0001_MethodMixed,
        Diagnostics.CQ0002_LogicInComposition,
        Diagnostics.CQ0003_CompositionInLogic
    );

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
    }
}
