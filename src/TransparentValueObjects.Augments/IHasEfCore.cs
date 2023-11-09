using JetBrains.Annotations;

namespace TransparentValueObjects.Augments;

/// <summary>
/// Augment to enable support for EF Core model configuration.
/// </summary>
/// <typeparam name="TValueObject"></typeparam>
/// <typeparam name="TValue"></typeparam>
[PublicAPI]
public interface IHasEfCore<out TValueObject, out TValue>
    where TValueObject : IValueObject<TValue>
    where TValue : notnull
{

}
