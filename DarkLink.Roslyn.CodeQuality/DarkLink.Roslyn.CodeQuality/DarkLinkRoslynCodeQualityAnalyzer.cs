using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

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

        context.RegisterOperationBlockStartAction(analysisContext =>
        {
            var logicNodes = new List<IOperation>();
            var compositionNodes = new List<IInvocationOperation>();

            analysisContext.RegisterOperationAction(operationAnalysisContext => { logicNodes.Add(operationAnalysisContext.Operation); },
                OperationKind.Branch,
                OperationKind.Conditional,
                OperationKind.ConditionalAccess,
                OperationKind.ConditionalAccessInstance);

            analysisContext.RegisterOperationAction(operationAnalysisContext => { compositionNodes.Add((IInvocationOperation) operationAnalysisContext.Operation); },
                OperationKind.Invocation);

            analysisContext.RegisterOperationBlockEndAction(blockAnalysisContext =>
            {
                if (logicNodes.Count == 0 || compositionNodes.Count == 0) return;

                if (logicNodes.Count > compositionNodes.Count)
                    compositionNodes.ForEach(operation => blockAnalysisContext.ReportDiagnostic(Diagnostic.Create(Diagnostics.CQ0003_CompositionInLogic, operation.Syntax.GetLocation(), operation.TargetMethod.Name)));

                if (compositionNodes.Count > logicNodes.Count)
                    logicNodes.ForEach(operation => blockAnalysisContext.ReportDiagnostic(Diagnostic.Create(Diagnostics.CQ0002_LogicInComposition, operation.Syntax.GetLocation(), operation.Kind)));

                foreach (var location in blockAnalysisContext.OwningSymbol.Locations)
                    blockAnalysisContext.ReportDiagnostic(Diagnostic.Create(Diagnostics.CQ0001_MethodMixed, location, blockAnalysisContext.OwningSymbol.Name));
            });
        });
    }
}
