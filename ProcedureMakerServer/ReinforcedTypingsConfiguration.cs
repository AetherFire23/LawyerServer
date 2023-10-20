using Reinforced.Typings;
using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Fluent;
using Config = Reinforced.Typings.Fluent.ConfigurationBuilder;
namespace ProcedureMakerServer;

public static class ReinforcedTypingsConfiguration
{
    public static void Configure(Config builder)
    {
        builder.Global(config => config.CamelCaseForProperties()
            .AutoOptionalProperties()
            .UseModules());

        builder.Substitute(typeof(Guid), new RtSimpleTypeName("string"));

    }
}
