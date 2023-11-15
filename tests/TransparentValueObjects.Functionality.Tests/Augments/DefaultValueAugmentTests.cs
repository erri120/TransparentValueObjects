using System;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Functionality.Tests.Augments;

public partial class DefaultValueAugmentTests
{
    [ValueObject<Guid>]
    private readonly partial struct ValueObjectWithDefaultValue : IAugmentWith<DefaultValueAugment>
    {
        public static ValueObjectWithDefaultValue DefaultValue { get; } = From(Guid.Parse("9566068b-a89e-4d1a-a404-b9292d100a23"));
    }

    [ValueObject<Guid>]
    private readonly partial struct ValueObjectWithoutDefaultValue { }

    [Fact]
    public void Test_DefaultValue()
    {
        ValueObjectWithDefaultValue.DefaultValue.Value.Should().Be("9566068b-a89e-4d1a-a404-b9292d100a23");
    }

    [Fact]
    public void Test_PublicConstructor_WithoutAugment()
    {
        var act = () =>
        {
            try
            {
                Activator.CreateInstance<ValueObjectWithoutDefaultValue>();
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException is not null)
                    throw e.InnerException;
            }
        };

        act.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void Test_PublicConstructor_WithAugment()
    {
        var vo = new ValueObjectWithDefaultValue();
        vo.Value.Should().Be("9566068b-a89e-4d1a-a404-b9292d100a23");
    }
}
