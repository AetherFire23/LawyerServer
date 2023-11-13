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
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ProcedureContext>();

            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }
    }
    public static async Task ConfigureCors(WebApplication app)
    {
        app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());
    }
}
