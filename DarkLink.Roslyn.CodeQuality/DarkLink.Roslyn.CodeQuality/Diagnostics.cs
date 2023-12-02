using System;
using Microsoft.CodeAnalysis;

namespace DarkLink.Roslyn.CodeQuality;

public static class Diagnostics
{
    private const string Category = "CodeQuality";

    public static readonly DiagnosticDescriptor CQ0001_MethodMixed = CreateDescriptor(
        "CQ0001",
        nameof(Resources.CQ0001_Title),
        nameof(Resources.CQ0001_MessageFormat),
        nameof(Resources.CQ0001_Description));

    public static readonly DiagnosticDescriptor CQ0002_LogicInComposition = CreateDescriptor(
        "CQ0002",
        nameof(Resources.CQ0002_Title),
        nameof(Resources.CQ0002_MessageFormat),
        nameof(Resources.CQ0002_Description));

    public static readonly DiagnosticDescriptor CQ0003_CompositionInLogic = CreateDescriptor(
        "CQ0003",
        nameof(Resources.CQ0003_Title),
        nameof(Resources.CQ0003_MessageFormat),
        nameof(Resources.CQ0003_Description));

    private static DiagnosticDescriptor CreateDescriptor(
        string id,
        string titleResource,
        string messageTitleResource,
        string descriptionResource) =>
        new(
            id,
            CreateLocalizableString(titleResource),
            CreateLocalizableString(messageTitleResource),
            Category,
            DiagnosticSeverity.Warning,
            true,
            CreateLocalizableString(descriptionResource));

    private static LocalizableString CreateLocalizableString(string resourceName) =>
        new LocalizableResourceString(resourceName, Resources.ResourceManager, typeof(Resources));
}
