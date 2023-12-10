namespace ProcedureMakerServer.Initialization;
public static class TestScope
{
    public static async Task Run(WebApplication app)
    {
        using (IServiceScope scope = app.Services.CreateScope())
        {

           
        }
    }

    public static async Task CreateUser(IServiceScope scope)
    {
    }
}
