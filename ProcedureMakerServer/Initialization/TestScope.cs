using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Interfaces;
using Newtonsoft.Json;
using ProcedureMakerServer.Models;
using OneOf;
using ProcedureMakerServer.Authentication.ReturnModels;

namespace ProcedureMakerServer.Initialization;
public static class TestScope
{
    public static async Task Run(WebApplication app)
    {
        using (IServiceScope scope = app.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            var lawyerRepo = scope.ServiceProvider.GetRequiredService<ILawyerRepository>();
            var context = scope.ServiceProvider.GetRequiredService<ProcedureContext>();
            var caseContextService = scope.ServiceProvider.GetRequiredService<ICaseContextService>();
            var auth = provider.GetRequiredService<IAuthManager>();

            RegisterRequest req = new RegisterRequest()
            {
                Password = "password",
                Role = Authentication.RoleTypes.Admin,
                Username = "fred"
            };



            await auth.TryRegister(req);

            await context.SaveChangesAsync();
            LoginRequest loginRequest = new LoginRequest()
            {
                Password = "password",
                Username = "fred",
            };

            OneOf<FailedLoginResult, SuccessLoginResult> loginResult = await auth.GenerateTokenIfCorrectCredentials(loginRequest);

            SuccessLoginResult success = loginResult.AsT1;

            // now test crud capabilities
            // besoin du User icitte


            CaseCreationInfo caseCreation = new CaseCreationInfo()
            {
                CaseNumber = "200-04-555-222",
                ClientFirstName = "Fred",
                ClientLastName = "Richer",
                LawyerId = success.UserDto.Lawyer.Id,
            };

            await caseContextService.CreateNewCase(caseCreation);


            var lcase = await caseContextService.GetCase(success.UserDto.Lawyer.Id);

            lcase.Cases.First().Client.FirstName = "I am changed!";

            await caseContextService.SaveContextDto(lcase);

           // caseContextService.CreateNewCase();



        }
    }
}
