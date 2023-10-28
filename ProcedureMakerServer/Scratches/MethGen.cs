using Reinforced.Typings;
using Reinforced.Typings.Ast;
using Reinforced.Typings.Generators;
using System.Reflection;

namespace ProcedureMakerServer.Scratches;

public class MethGen : MethodCodeGenerator
{
    public override RtFunction GenerateNode(MethodInfo element, RtFunction result, TypeResolver resolver)
    {
        result.Body = new RtRaw("dick");

        return result;
    }
}
