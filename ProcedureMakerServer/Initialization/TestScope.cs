using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Models;

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

            var result = await auth.GenerateTokenIfCorrectCredentials(loginRequest);


            // var loginResult = JsonConvert.DeserializeObject<LoginResult>(result.SerializedData);

            var logResult = result;


            CaseCreationInfo caseCreation = new CaseCreationInfo()
            {
                CaseNumber = "200-04-555-222",
                ClientFirstName = "Fred",
                ClientLastName = "Richer",
                LawyerId = logResult.UserDto.LawyerId,
            };

            await caseContextService.CreateNewCase(caseCreation);


            var lcase = await caseContextService.GetCaseContext(logResult.UserDto.LawyerId);

            lcase.Cases.First().Client.FirstName = "I am changed!";

            await caseContextService.SaveCaseDto(lcase.Cases.First());

            // caseContextService.CreateNewCase();



        }
    }
}
