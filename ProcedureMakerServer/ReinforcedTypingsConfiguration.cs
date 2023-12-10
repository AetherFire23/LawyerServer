using ProcedureMakerServer.Constants;
using ProcedureMakerServer.Scratches;
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
        builder.Substitute(typeof(DateTime), new RtSimpleTypeName("Date"));

        builder.ExportAsClass<UserEndpoints>()
            .WithFields(typeof(UserEndpoints).GetFields().ToList(), x =>
            {
                x.WithFieldCodeGenerator<ConstCodeGen>();
            });

        builder.ExportAsClass<CasesEndpoints>()
            .WithFields(typeof(CasesEndpoints).GetFields().ToList(), x =>
            {
                x.WithFieldCodeGenerator<ConstCodeGen>();
            });
    }
}
