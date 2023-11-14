using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using VerifyXunit;

namespace TransparentValueObjects.Tests;

public static class TestHelpers
{
    public static Task Verify(string target, [CallerFilePath] string sourceFile = "")
    {
        // ReSharper disable once ExplicitCallerInfoArgument
        return Verifier.Verify(target, extension: "cs", sourceFile: sourceFile);
    }

    public static Task VerifyGenerator(string hintName, [LanguageInjection("csharp")] string input, [CallerFilePath] string sourceFile = "")
    {
        var res = RunGenerator(hintName, input);
        // ReSharper disable once ExplicitCallerInfoArgument
        return Verify(res, sourceFile: sourceFile);
    }

    public static void TestPostInitializationOutput(string hintName, [LanguageInjection("csharp")] string expected)
    {
        TestGenerator(hintName, Array.Empty<SyntaxTree>(), expected);
    }

    public static Task VerifyPostInitializationOutput(string hintName, [CallerFilePath] string sourceFile = "")
    {
        var res = RunGenerator(hintName, Array.Empty<SyntaxTree>());
        // ReSharper disable once ExplicitCallerInfoArgument
        return Verify(res, sourceFile: sourceFile);
    }

    private static string RunGenerator(string hintName, IEnumerable<SyntaxTree> syntaxTrees)
    {
        var (driver, compilation) = SetupGenerator(syntaxTrees);

        var runResult = driver.RunGenerators(compilation).GetRunResult();
        runResult.Results.Should().ContainSingle();

        var result = runResult.Results[0];
        result.Exception.Should().BeNull();

        result.GeneratedSources.Should().Contain(sourceResult => string.Equals(sourceResult.HintName, hintName, StringComparison.Ordinal));
        var res = result.GeneratedSources.First(sourceResult => string.Equals(sourceResult.HintName, hintName, StringComparison.Ordinal));
        return res.SourceText.ToString();
    }

    public static string RunGenerator(string hintName, [LanguageInjection("csharp")] string input)
    {
        return RunGenerator(hintName, new[] { CSharpSyntaxTree.ParseText(input) });
    }

    public static Task VerifyRegion(string source, string regionName, [CallerFilePath] string sourceFile = "")
    {
        var region = GetRegion(source, regionName);
        // ReSharper disable once ExplicitCallerInfoArgument
        return Verify(region, sourceFile: sourceFile);
    }

    public static string GetRegion(string source, string regionName)
    {
        var span = source.AsSpan();
        var startSearch = $"#region {regionName}";
        var endSearch = $"#endregion {regionName}";

        var startIndex = span.IndexOf(startSearch, StringComparison.Ordinal);
        startIndex.Should().BeGreaterThanOrEqualTo(0);

        span = span[startIndex..];

        var endIndex = span.IndexOf(endSearch, StringComparison.Ordinal);
        endIndex.Should().BeGreaterThanOrEqualTo(0);

        span = span[..(endIndex + endSearch.Length)];
        return span.ToString().SourceNormalize();
    }

    public static Diagnostic[] GetDiagnostics([LanguageInjection("csharp")] string input)
    {
        var (driver, compilation) = SetupGenerator(new[] { CSharpSyntaxTree.ParseText(input) });

        var runResult = driver.RunGenerators(compilation).GetRunResult();
        return runResult.Diagnostics.ToArray();
    }

    private static void TestGenerator(string hintName, IEnumerable<SyntaxTree> syntaxTrees, [LanguageInjection("csharp")] string expected)
    {
        var (driver, compilation) = SetupGenerator(syntaxTrees);

        var runResult = driver.RunGenerators(compilation).GetRunResult();
        runResult.Results.Should().ContainSingle();

        var result = runResult.Results[0];
        result.Exception.Should().BeNull();

        result.GeneratedSources
            .Should()
            .Contain(sourceResult => string.Equals(sourceResult.HintName, hintName, StringComparison.Ordinal))
            .Which
            .SourceText
            .ToString()
            .SourceNormalize()
            .Should()
            .Be(expected.SourceNormalize());
    }

    private static (CSharpGeneratorDriver driver, CSharpCompilation compilation) SetupGenerator(IEnumerable<SyntaxTree> syntaxTrees)
    {
        var generator = new ValueObjectIncrementalSourceGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CSharpCompilation.Create(nameof(TestHelpers),
            syntaxTrees,
            new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            });

        return (driver, compilation);
    }

    public static string SourceNormalize(this string input)
    {
        var sb = new StringBuilder(input);
        sb.Replace("\r\n", "\n");
        sb.Replace("    ", "\t");
        return sb.ToString().Trim();
    }
}
