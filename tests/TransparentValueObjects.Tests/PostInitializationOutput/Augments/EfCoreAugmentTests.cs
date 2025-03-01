using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.PostInitializationOutput.Augments;

public class EfCoreAugmentTests
{
    [Fact]
    public Task Test_Source()
    {
        return TestHelpers.VerifyPostInitializationOutput("EfCoreAugment.g.cs");
    }
}
