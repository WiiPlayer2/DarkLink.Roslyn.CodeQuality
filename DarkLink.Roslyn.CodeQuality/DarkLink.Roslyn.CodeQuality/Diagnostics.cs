using System;
using Microsoft.CodeAnalysis;

namespace DarkLink.Roslyn.CodeQuality;

public static class Diagnostics
{
    private const string Category = "Naming";

    public static readonly DiagnosticDescriptor Rule = CreateDescriptor(
        "DarkLinkRoslynCodeQuality",
        nameof(Resources.AnalyzerTitle),
        nameof(Resources.AnalyzerMessageFormat),
        nameof(Resources.AnalyzerDescription));

    private static DiagnosticDescriptor CreateDescriptor(
        string id,
        string titleResource,
        string messageResource,
        string descriptionResource) =>
        new(
            id,
            CreateLocalizableString(titleResource),
            CreateLocalizableString(messageResource),
            Category,
            DiagnosticSeverity.Warning,
            true,
            CreateLocalizableString(descriptionResource));

    private static LocalizableString CreateLocalizableString(string resourceName) =>
        new LocalizableResourceString(resourceName, Resources.ResourceManager, typeof(Resources));
}
