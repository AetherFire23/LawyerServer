using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Interfaces;



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
            var auth = provider.GetRequiredService<IAuthManager>();

            RegisterRequest req = new RegisterRequest()
            {
                Password = "password",
                Role = Authentication.RoleTypes.Admin,
                Username = "fred"
            };

            string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(req);

            await auth.TryRegister(req);



            LoginRequest loginRequest = new LoginRequest()
            {
                Password = "password",
                Username = "fred",
            };

            await auth.GenerateTokenIfCorrectCredentials(loginRequest);


            // now test crud capabilities


        }
    }
}
