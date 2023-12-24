using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class XEnumNamesSchemaFilter : ISchemaFilter
{
    private const string NAME = "x-enumNames";

    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        Type typeInfo = context.Type;
        // Chances are something in the pipeline might generate this automatically at some point in the future
        // therefore it's best to check if it exists.
        if (typeInfo.IsEnum && !model.Extensions.ContainsKey(NAME))
        {
            string[] names = Enum.GetNames(context.Type);
            OpenApiArray arr = new OpenApiArray();
            arr.AddRange(names.Select(name => new OpenApiString(name)));
            model.Extensions.Add(NAME, arr);
        }
    }
}