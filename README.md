# TransparentValueObjects

Source generator and analyzer to create Value Objects.

## Example

```csharp
using System;
using TransparentValueObjects;

[ValueObject<Guid>]
public readonly partial struct MyId { }
```

The attribute `ValueObject<TInnerValue>` will generate a partial implementation of this readonly struct with the following defaults:

- A `public readonly TInnerValue Value` field.
- A `public static T From(TInnerValue innerValue)` method that uses the private constructor.
- A private constructor used by the `From` method.
- A public constructor marked as `Obsolete` with `error: true` that will throw an exception if called (this behavior can be overwritten using [Augments](#augments)).
- `GetHashCode` and `ToString` implementations that call the same methods on the inner value.
- Equality methods: `object.Equals`, `IEquatable<T>` and `IEquatable<TInnerValue>`.
  - `==` and `!=` operators.
  - Additional `Equals` method with `IEqualityComparer<TInnerValue>` parameter.
- Explicit cast operators.
- `IComparable<T>` and `IComparable<TInnerValue>` if `TInnerValue` implements `IComparable<TInnerValue>`.
  - `<`, `<=`, `>` and `>=` operators.
- For `Guid` only: `NewId` method that calls `From(Guid.NewGuid())`.

You can check the [test files](./tests/TransparentValueObjects.Tests/SourceOutput) to view the generated output.

## Augments

The biggest selling point of this project is the augment feature. You can use the `IAugmentWith` interface to "augment" your Value Object with additional functionality:

```csharp
using System;
using TransparentValueObjects;

[ValueObject<Guid>]
public readonly partial struct SampleGuidValueObject : IAugmentWith<
    DefaultValueAugment,
    JsonAugment,
    EfCoreAugment>
{
    /// <inheritdoc/>
    public static SampleGuidValueObject DefaultValue => From(Guid.Empty);
}
```

The following augments are currently available:

- [`DefaultValueAugment`](#default-value)
- [`DefaultEqualityComparerAugment`](#default-equality-comparer)
- [`JsonAugment`](#json-augment)
- [`EfCoreAugment`](#ef-core-augment)

### Default Value

Augments the Value Object with the `IDefaultValue` interface that has a static member `DefaultValue`:

```csharp
public static abstract TValueObject DefaultValue { get; }
```

```csharp
using System;
using TransparentValueObjects;

[ValueObject<Guid>]
public readonly partial struct SampleGuidValueObject : IAugmentWith<DefaultValueAugment>
{
    /// <inheritdoc/>
    public static SampleGuidValueObject DefaultValue => From(Guid.Empty);
}
```

You have to implement the `DefaultValue` member when using this augment. The public constructor will now also be available:

```csharp
public SampleGuidValueObject()
{
    Value = DefaultValue.Value;
}
```

### Default Equality Comparer

Augments the Value Object with the `IDefaultEqualityComparer` interface that has a static member `InnerValueDefaultEqualityComparer`:

```csharp
public static abstract IEqualityComparer<TInnerValue> InnerValueDefaultEqualityComparer { get; }
```

```csharp
[ValueObject<string>]
public readonly partial struct SampleStringValueObject : IAugmentWith<DefaultEqualityComparerAugment>
{
    /// <inheritdoc/>
    public static IEqualityComparer<string> InnerValueDefaultEqualityComparer => StringComparer.OrdinalIgnoreCase;
}
```

This default equality comparer will be used by the following methods:

```csharp
public bool Equals(SampleStringValueObject other) => Equals(other.Value);

public bool Equals(string? other) => InnerValueDefaultEqualityComparer.Equals(Value, other);

public override int GetHashCode() => InnerValueDefaultEqualityComparer.GetHashCode(Value);
```

This augment is especially great for strings if you want to always use the case-insensitive equality comparer.

### Json Augment

Augments the Value Object with a JSON converter:

```csharp
[ValueObject<string>]
public readonly partial struct SampleStringValueObject : IAugmentWith<JsonAugment> { }
```

**Only** `System.Text.Json` is currently supported! See https://github.com/erri120/TransparentValueObjects/issues/11 for `Newtonsoft.Json` support.

```csharp
[JsonConverter(typeof(JsonConverter))]
readonly partial struct SampleStringValueObject
{
    public class JsonConverter : JsonConverter<SampleStringValueObject> { /* omitted */ }
}
```

The `JsonConverterAttribute` will be added to the Value Object, meaning that you can use the added converter without needing to manually add it to the JSON options.

The source generator will create a custom converter for the following types:

- `string`
- `Guid`
- `Int16`
- `Int32`
- `Int64`
- `UInt16`
- `UInt32`
- `UInt64`

All other types will use a fallback converter that fetches an existing converter for `TInnerValue`.

**Note:** If the inner value type is a reference type, like `string`, then you will **need** to also augment the Value Object with the `DefaultValueAugment`.

### EF Core Augment

Augments the Value Object with a `ValueConverter<T, TInnerValue>` and `ValueComparer<T>`:

```csharp
[ValueObject<string>]
public readonly partial struct SampleStringValueObject : IAugmentWith<EfCoreAugment> { }
```

```csharp
public class EfCoreValueConverter : ValueConverter<SampleStringValueObject, string>
{
    public EfCoreValueConverter() : this(mappingHints: null) { }

    public EfCoreValueConverter(ConverterMappingHints? mappingHints = null) : base(
        static value => value.Value,
        static innerValue => From(innerValue),
        mappingHints
    ) { }
}

public class EfCoreValueComparer : ValueComparer<SampleStringValueObject>
{
    public EfCoreValueComparer() : base(
        static (left, right) => left.Equals(right),
        static value => value.GetHashCode(),
        static value => From(value.Value)
    ) { }

    /// <inheritdoc/>
    public override bool Equals(SampleStringValueObject left, SampleStringValueObject right) => left.Equals(right);

    /// <inheritdoc/>
    public override SampleStringValueObject Snapshot(SampleStringValueObject instance) => From(instance.Value);

    /// <inheritdoc/>
    public override int GetHashCode(SampleStringValueObject instance) => instance.GetHashCode();
}
```
