using ProcedureMakerServer.Constants;
using ProcedureMakerServer.Scratches;
using Reinforced.Typings.Ast.TypeNames;
using Reinforced.Typings.Fluent;
using System.Diagnostics;
using System.Reflection;
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

        //builder.ExportAsClass<UserEndpoints>()
        //    .WithMethods(typeof(UserEndpoints).GetMethods(), x =>
        //    {
        //        x.WithCodeGenerator<MethGen>();
        //    });


        var classes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.GetCustomAttribute<TsConstantClassAttribute>() is not null)
            .ToList();
        //   x.WithCodeGenerator<ConstClassCode>();

        //   x.OverrideNamespace("sex");

        //Trace.WriteLine(classes);
        //builder.ExportAsClasses(classes, x =>
        //{
        //});


    }
}
