using System.Linq;
using System.Text;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TransparentValueObjects.Augments;

namespace TransparentValueObjects.Tests;

public static class TestHelpers
{
    public static void TestGenerator(string input, string fileName, string expectedOutput)
    {
        var (driver, compilation) = SetupGenerator(new[] { CSharpSyntaxTree.ParseText(input) });

        var runResult = driver.RunGenerators(compilation).GetRunResult();
        var generated = runResult.GeneratedTrees.FirstOrDefault(t => t.FilePath.EndsWith(fileName, System.StringComparison.Ordinal));
        generated.Should().NotBeNull();

        NormalizeEquals(generated!.GetText().ToString(), expectedOutput);
    }

    public static (CSharpGeneratorDriver driver, CSharpCompilation compilation) SetupGenerator(SyntaxTree[] syntaxTrees)
    {
        var generator = new ValueObjectIncrementalSourceGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CSharpCompilation.Create(nameof(TestHelpers),
            syntaxTrees,
            new[]
            {
                // To support 'System.Attribute' inheritance, add reference to 'System.Private.CoreLib'.
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),

                // augments
                MetadataReference.CreateFromFile(typeof(Marker).Assembly.Location)
            });

        return (driver, compilation);
    }

    public static string Normalize(string input)
    {
        var sb = new StringBuilder(input);
        sb.Replace("\r\n", "\n");
        sb.Replace("    ", "\t");
        return sb.ToString().Trim();
    }

    public static void NormalizeEquals(string actual, string expected)
    {
        Normalize(actual).Should().Be(Normalize(expected));
    }
}
