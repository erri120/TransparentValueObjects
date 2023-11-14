using System;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Sample.Tests.Augments;

[ValueObject<Guid>]
public readonly partial struct ValueObjectWithDefaultValue : IAugmentWith<DefaultValueAugment>
{
    public static ValueObjectWithDefaultValue DefaultValue { get; } = From(Guid.Parse("9566068b-a89e-4d1a-a404-b9292d100a23"));
}

[ValueObject<Guid>]
public readonly partial struct ValueObjectWithoutDefaultValue { }

public class DefaultValueAugmentTests
{
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
