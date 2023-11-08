using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using ProcedureMakerServer.Controllers;
using System.Reflection;
using System.Text;

namespace ProcedureMakerServer.Scratches;
public static class RTKQueryExporter
{
    public static void ExportQueriesToFile()
    {
        var methods = typeof(UserController)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.GetCustomAttribute<HttpMethodAttribute>() is not null)
            .Where(x => x.GetCustomAttribute<DefaultReturnTypeAttribute>() is not null)
            .ToList();

        var queryInfos = methods.Select(x => new ReflectedQuery(x)).ToList();

        var queries = queryInfos.Select(x => new QueryCodeBuilder(x));

        // was building the query :)

        //

    }
}

public class ReflectedQuery
{
    public string HttpMethodType { get; set; }
    public string Name { get; set; }

    public string BodyTypeName { get; set; } = string.Empty;
    public List<(string, string)> ParameterTypeNames { get; set; }


    public bool HasBody => BodyTypeName != string.Empty;

    public string ReturnTypeName { get; set; }

    public string Path { get; set; }

    private MethodInfo _methodInfo;
    private HttpMethodAttribute _httpAttribute;

    public ReflectedQuery(MethodInfo methodInfo)
    {
        _methodInfo = methodInfo;
        _httpAttribute = _methodInfo.GetCustomAttribute<HttpMethodAttribute>();
        HttpMethodType = _httpAttribute.HttpMethods.First();
        Path = _httpAttribute.Template;
        BodyTypeName = GetBodyTypeName(); // gonna need to lowerfirst letter

        // is Task<IActionResult> 
        ReturnTypeName = _methodInfo.GetCustomAttribute<DefaultReturnTypeAttribute>().ReturnType.Name;
        Name = _methodInfo.Name;

        ParameterTypeNames = _methodInfo.GetParameters()
            .Where(x=> x.GetCustomAttribute<FromBodyAttribute>() is null)
            .Select(x => (x.ParameterType.Name, x.Name))
            .ToList();
    }

    public string GetBodyTypeName()
    {
        var bodyParams = _methodInfo.GetParameters()
            .Where(x => x.GetCustomAttribute<FromBodyAttribute>() is not null)
            .ToList();

        if (!bodyParams.Any()) return string.Empty;

        string name = bodyParams.First().ParameterType.Name;
        return name;
    }
}

public class QueryCodeBuilder
{
    public string QueryLine { get; set; }
    private ReflectedQuery _query { get; }
    public QueryCodeBuilder(ReflectedQuery query)
    {
        _query = query;

        QueryLine = GenerateQueryLine();
    }

    private string GenerateQueryLine()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.Append($"{_query.Name}");
        stringBuilder.Append($": builder.{GetBuilderOrMutation()}");
        stringBuilder.Append($"<{GetReturnAndArgumentLine()}>");
        stringBuilder.Append(GetReturnAndArgumentLine());

        return string.Empty;
    }

    private string GetBuilderOrMutation()
    {
        var mutationMethodNames = new List<string>
        {
            "PUT",
            "POST",
            "DELETE"
        };

        string queryType = mutationMethodNames.Contains(_query.HttpMethodType)
            ? "mutation"
            : "query";

        return queryType;
    }

    private string GetReturnAndArgumentLine()
    {
        string s = $"<{_query.ReturnTypeName}, >";
        return string.Empty;
    }
}

public class DefaultReturnTypeAttribute : Attribute
{
    public readonly Type ReturnType;
    public DefaultReturnTypeAttribute(Type returnType)
    {
        this.ReturnType = returnType;
    }
}