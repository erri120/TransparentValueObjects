using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.PostInitializationOutput;

public class ValueObjectAttributeTests
{
    [Fact]
    public Task Test_Source()
    {
        return TestHelpers.VerifyPostInitializationOutput("ValueObjectAttribute.g.cs");
    }
}
