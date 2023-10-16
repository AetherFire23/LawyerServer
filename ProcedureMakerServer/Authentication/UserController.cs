using Microsoft.AspNetCore.Authorization;
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

    [HttpPut("token")]
    public async Task<IActionResult> GenerateTokenIfValid([FromBody] LoginRequest loginRequest)
    {
        return (await _authManager.GenerateTokenIfCorrectCredentials(loginRequest)).Match(Ok, Ok);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest registerRequest)
    {
        var result = (await _authManager.TryRegister(registerRequest)).Match(Ok, Ok);
        return result;
    }

    [HttpGet("test")]
    [Authorize(Roles = nameof(RoleTypes.Admin))]
    public async Task<IActionResult> AuthorizedTest()
    {
        return Ok();
    }
}