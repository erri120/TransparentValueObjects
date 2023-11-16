using System;
using FluentAssertions;
using Xunit;

namespace TransparentValueObjects.Functionality.Tests.Augments;

public partial class EfCoreAugmentTests
{
    [ValueObject<Guid>]
    private readonly partial struct MyGuidValueObject : IAugmentWith<EfCoreAugment> { }

    [ValueObject<string>]
    private readonly partial struct MyStringValueObject : IAugmentWith<EfCoreAugment> { }

    /// <summary>
    ///
    /// </summary>
    [Fact]
    public void Test_ValueConverter_ValueType()
    {
        var innerValue = Guid.NewGuid();
        var vo = MyGuidValueObject.From(innerValue);

        var converter = new MyGuidValueObject.EfCoreValueConverter();

        converter.ModelClrType.Should().Be(typeof(MyGuidValueObject));
        converter.ProviderClrType.Should().Be(typeof(Guid));

        converter.ConvertToProvider.Invoke(vo).Should().Be(innerValue);
        converter.ConvertFromProvider.Invoke(innerValue).Should().Be(vo);
    }

    [Fact]
    public void Test_ValueConverter_ReferenceType()
    {
        const string innerValue = "Hello World!";
        var vo = MyStringValueObject.From(innerValue);

        var converter = new MyStringValueObject.EfCoreValueConverter();

        converter.ModelClrType.Should().Be(typeof(MyStringValueObject));
        converter.ProviderClrType.Should().Be(typeof(string));

        converter.ConvertToProvider.Invoke(vo).Should().Be(innerValue);
        converter.ConvertFromProvider.Invoke(innerValue).Should().Be(vo);
    }

    [Fact]
    public void Test_ValueComparer_ValueType()
    {
        var innerValue1 = Guid.NewGuid();
        var innerValue2 = Guid.NewGuid();

        var vo1 = MyGuidValueObject.From(innerValue1);
        var vo2 = MyGuidValueObject.From(innerValue2);

        var comparer = new MyGuidValueObject.EfCoreValueComparer();

        comparer.Equals(vo1, vo2).Should().BeFalse();
        comparer.Equals(vo1, vo1).Should().BeTrue();

        comparer.GetHashCode(vo1).Should().Be(vo1.GetHashCode());

        comparer.Snapshot(vo1).Should().Be(vo1);
    }

    [Fact]
    public void Test_ValueComparer_ReferenceType()
    {
        const string innerValue1 = "Hi";
        const string innerValue2 = "Hello World";

        var vo1 = MyStringValueObject.From(innerValue1);
        var vo2 = MyStringValueObject.From(innerValue2);

        var comparer = new MyStringValueObject.EfCoreValueComparer();

        comparer.Equals(vo1, vo2).Should().BeFalse();
        comparer.Equals(vo1, vo1).Should().BeTrue();

        comparer.GetHashCode(vo1).Should().Be(vo1.GetHashCode());

        var snapshot = comparer.Snapshot(vo1);
        snapshot.Should().Be(vo1);
    }
}
