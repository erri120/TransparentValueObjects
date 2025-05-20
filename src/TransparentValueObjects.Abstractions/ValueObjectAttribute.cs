using System;
using JetBrains.Annotations;

namespace TransparentValueObjects;

/// <summary>
/// Indicates that the struct is a value object.
/// </summary>
/// <typeparam name="TInnerValue">The type of the inner value.</typeparam>
[PublicAPI]
[AttributeUsage(validOn: AttributeTargets.Struct)]
public class ValueObjectAttribute<TInnerValue> : Attribute
    where TInnerValue : notnull;
