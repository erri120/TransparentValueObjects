using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.SourceOutput.Augments;

[UsesVerify]
public class EfCoreAugmentTests
{
    [Fact]
    public Task Test_AddEfCoreConverterComparer()
    {
        var cw = new CodeWriter();
        ValueObjectIncrementalSourceGenerator.AddEfCoreConverterComparer(cw, "TestValueObject", "global::System.String");
        return TestHelpers.Verify(cw.ToString());
    }
}
