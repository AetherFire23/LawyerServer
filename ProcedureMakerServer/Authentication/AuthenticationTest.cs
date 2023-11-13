using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Models;

namespace ProcedureMakerServer.Authentication;

public static class AuthenticationTest
{
    public static async Task DoTest(IServiceScope scope)
    {
        var provider = scope.ServiceProvider;
        var context = scope.ServiceProvider.GetRequiredService<ProcedureContext>();
        var caseContextService = scope.ServiceProvider.GetRequiredService<ICaseContextService>();
        var auth = provider.GetRequiredService<IAuthManager>();

        RegisterRequest req = new RegisterRequest()
        {
            Password = "password",
            Role = Authentication.RoleTypes.Normal,
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

        CaseCreationInfo caseCreation = new CaseCreationInfo()
        {
            CaseNumber = "200-04-555-222",
            ClientFirstName = "Fred",
            ClientLastName = "Richer",
            LawyerId = result.UserDto.LawyerId,
        };

        await caseContextService.CreateNewCase(caseCreation);


        var lcase = await caseContextService.GetCaseContext(result.UserDto.LawyerId);

        lcase.Cases.First().Client.FirstName = "I am changed!";

        await caseContextService.SaveCaseDto(lcase.Cases.First());

    }
}
