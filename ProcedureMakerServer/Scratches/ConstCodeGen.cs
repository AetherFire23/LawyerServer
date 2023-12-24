namespace ProcedureMakerServer.Scratches;

//public class ConstCodeGen : FieldCodeGenerator
//{
//    public override RtField GenerateNode(MemberInfo element, RtField result, TypeResolver resolver)
//    {
//        result.IsStatic = true;
//        result.Type = resolver.ResolveTypeName(typeof(string));

//        // to apply camelCasing
//        result.Identifier = new RtIdentifier(element.Name.LowerCaseFirstLetter());

//        // to apply aspnet conventions

//        if (element.Name == "Path")
//        {
//            var pathName = (string)element.DeclaringType.GetField("Path").GetValue(element.ReflectedType);

//            result.InitializationExpression = $"\"{pathName}\"";
//        }
//        else
//        {

//            string className = element.DeclaringType.Name;

//            var builder = new StringBuilder();
//            builder.Append("`${");
//            builder.Append(className);
//            builder.Append(".path}/");
//            builder.Append($"{element.Name.ToLower()}`");
//            result.InitializationExpression = builder.ToString();
//        }

//        return result;
//    }
//}
