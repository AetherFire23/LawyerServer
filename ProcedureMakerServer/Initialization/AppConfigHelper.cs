using Microsoft.EntityFrameworkCore;

namespace ProcedureMakerServer.Initialization;




// should make a <<persist>> mode
// so that tokens and their corresponding player ids remain valid
public static class AppConfigHelper
{
    public static async Task ConfigureApp(WebApplication app)
    {
        await ConfigureCors(app);
        await ConfigureMigration(app);

        await TestScope.Run(app);
    }
    public static async Task ConfigureMigration(WebApplication app)
    {
        using (IServiceScope scope = app.Services.CreateScope())
        {
            ProcedureContext context = scope.ServiceProvider.GetRequiredService<ProcedureContext>();

            _ = context.Database.EnsureDeleted();
            context.Database.Migrate();
        }
    }
    public static async Task ConfigureCors(WebApplication app)
    {
        _ = app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());
    }
}
