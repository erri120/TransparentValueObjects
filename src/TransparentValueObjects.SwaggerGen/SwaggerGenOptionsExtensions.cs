using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TransparentValueObjects.SwaggerGen;

public static class SwaggerGenOptionsExtensions
{
    public static void AddTransparentValueObjects(this SwaggerGenOptions options)
    {
        options.SchemaFilter<TransparentValueObjectSchemaFilter>();
    }
}
