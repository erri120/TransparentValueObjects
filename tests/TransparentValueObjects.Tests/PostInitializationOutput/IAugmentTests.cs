using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.PostInitializationOutput;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class IAugmentTests
{
    [Fact]
    public Task Test_Source()
    {
        return TestHelpers.VerifyPostInitializationOutput("IAugment.g.cs");
    }
}
