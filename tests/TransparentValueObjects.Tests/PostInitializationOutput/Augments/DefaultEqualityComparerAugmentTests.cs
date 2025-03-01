using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.PostInitializationOutput.Augments;

public class DefaultEqualityComparerAugmentTests
{
    [Fact]
    public Task Test_Source()
    {
        return TestHelpers.VerifyPostInitializationOutput("DefaultEqualityComparerAugment.g.cs");
    }
}
