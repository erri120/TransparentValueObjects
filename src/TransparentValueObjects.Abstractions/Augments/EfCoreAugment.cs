using JetBrains.Annotations;

namespace TransparentValueObjects;

/// <summary>
/// Augment to enable support for EF Core model configuration by generting a
/// ValueConverter and a ValueComparer.
/// </summary>
/// <remarks>
/// This requires <c>Microsoft.EntityFrameworkCore</c>.
/// </remarks>
[PublicAPI]
public sealed class EfCoreAugment : IAugment;
