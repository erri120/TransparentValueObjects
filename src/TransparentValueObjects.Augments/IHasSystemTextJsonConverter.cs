using System;
using JetBrains.Annotations;

namespace TransparentValueObjects.Augments;

/// <summary>
/// Augment to enable support for JSON serialization via System.Text.Json.
/// </summary>
[PublicAPI]
public interface IHasSystemTextJsonConverter
{
    /// <summary>
    /// This will be implemented by the source generator. Alternatively, you can use
    /// this to override the converter type.
    /// </summary>
    public static abstract Type SystemTextJsonConverterType { get; }
}
