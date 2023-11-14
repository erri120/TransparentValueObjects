using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.PostInitializationOutput;

[UsesVerify]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class IAugmentWithTests
{
    [Fact]
    public Task Test_Source()
    {
        return TestHelpers.VerifyPostInitializationOutput("IAugmentWith.g.cs");
    }
}
