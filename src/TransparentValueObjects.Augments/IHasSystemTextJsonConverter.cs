using System;

namespace TransparentValueObjects.Augments;

public interface IHasSystemTextJsonConverter
{
    public static abstract Type SystemTextJsonConverterType { get; }
}
