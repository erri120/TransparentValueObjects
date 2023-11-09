using System.Collections.Generic;
using JetBrains.Annotations;

namespace TransparentValueObjects.Augments;

[PublicAPI]
public interface IHasDefaultEqualityComparer<TValueObject, in TValue>
    where TValueObject : IValueObject<TValue>
    where TValue : notnull
{
    public static abstract IEqualityComparer<TValue> InnerValueDefaultEqualityComparer { get; }
}
