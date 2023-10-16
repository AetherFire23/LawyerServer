using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository;

namespace ProcedureMakerServer.Initialization;

public static class TestScope
{
    public static async Task Run(WebApplication app)
    {
        using (IServiceScope scope = app.Services.CreateScope())
        {
            await Do(scope);
            var lawyerRepo = scope.ServiceProvider.GetRequiredService<ILawyerRepository>();
            //var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();


            var context = scope.ServiceProvider.GetRequiredService<ProcedureContext>();


            // seed

           // context.Roles.Add(Seeder.AdminRole);
            //context.UserRoles.Add(Seeder.FredIsADmin);
//context.Users.Add(Seeder.FredUser);

          //  await context.SaveChangesAsync();


          //  var user = await userRepo.GetUserById(Seeder.FredUser.Id);

        }
    }

    private static async Task Do(IServiceScope scope)
    {

    }
}
