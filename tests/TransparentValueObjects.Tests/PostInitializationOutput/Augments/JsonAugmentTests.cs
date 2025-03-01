using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.PostInitializationOutput.Augments;

public class JsonAugmentTests
{
    [Fact]
    public Task Test_Source()
    {
        return TestHelpers.VerifyPostInitializationOutput("JsonAugment.g.cs");
    }
}
