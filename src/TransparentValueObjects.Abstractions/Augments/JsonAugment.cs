using JetBrains.Annotations;

namespace TransparentValueObjects;

/// <summary>
/// Augment to enable JSON support by generating a JSON converter for the value object.
/// </summary>
/// <remarks>
/// This only supports <c>System.Text.Json</c> at the moment.
/// </remarks>
[PublicAPI]
public sealed class JsonAugment : IAugment;
