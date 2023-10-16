using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;

namespace ProcedureMakerServer.Authentication;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly IAuthManager _authManager;

    public UserController(IAuthManager authManager)
    {
        _authManager = authManager;
    }

    [Route("crap")]
    public async Task<IActionResult> GenerateTokenIfValid(LoginRequest loginRequest)
    {
        return (await _authManager.GenerateTokenIfCorrectCredentials(loginRequest)).Match(Ok, Ok);
    }

    [Route("shit")]
    public async Task<IActionResult> RegisterUser(RegisterRequest registerRequest)
    {
        var result = (await _authManager.TryRegister(registerRequest)).Match(Ok, Ok);
        return result;
    }
}