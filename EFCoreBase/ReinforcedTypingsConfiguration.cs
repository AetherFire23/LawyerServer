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
    }
}
