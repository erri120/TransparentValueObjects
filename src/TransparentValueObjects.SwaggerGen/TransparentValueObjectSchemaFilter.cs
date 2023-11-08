using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TransparentValueObjects.Augments;

namespace TransparentValueObjects.SwaggerGen;

public sealed class TransparentValueObjectSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValueObject<>)) is not { } vogen)
            return;

        if (vogen.GetGenericArguments() is not [_, { } valueObject])
            return;

        var schemaValueObject = context.SchemaGenerator.GenerateSchema(valueObject, context.SchemaRepository, context.MemberInfo, context.ParameterInfo);
        CopyPublicProperties(schemaValueObject, schema);
    }

    private static void CopyPublicProperties<T>(T oldObject, T newObject) where T : class
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

        if (ReferenceEquals(oldObject, newObject)) return;

        var type = typeof(T);
        var propertyList = type.GetProperties(flags);
        if (propertyList.Length <= 0) return;

        foreach (var newObjProp in propertyList)
        {
            var oldProp = type.GetProperty(newObjProp.Name, flags)!;
            if (!oldProp.CanRead || !newObjProp.CanWrite) continue;

            var value = oldProp.GetValue(oldObject);
            newObjProp.SetValue(newObject, value);
        }
    }
}
