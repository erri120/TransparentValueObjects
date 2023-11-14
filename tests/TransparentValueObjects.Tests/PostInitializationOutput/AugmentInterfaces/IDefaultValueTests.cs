using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace TransparentValueObjects.Tests.PostInitializationOutput.AugmentInterfaces;

[UsesVerify]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class IDefaultValueTests
{
    [Fact]
    public Task Test_Source()
    {
        return TestHelpers.VerifyPostInitializationOutput("IDefaultValue.g.cs");
    }
}
