using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Authentication.ReturnModels;
using ProcedureMakerServer.Constants;
using ProcedureMakerServer.Exceptions.HttpResponseExceptions;
using System.Security.Claims;

namespace ProcedureMakerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly IAuthManager _authManager;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenManager _JwtTokenManager;

    public UserController(IAuthManager authManager,
                          IUserRepository userRepository,
                          IJwtTokenManager jwtTokenManager)
    {
        _authManager = authManager;
        _userRepository = userRepository;
        _JwtTokenManager = jwtTokenManager;
    }

    [HttpPut(UserEndpoints.CredentialsLogin)]
    public async Task<IActionResult> GenerateTokenIfValid([FromBody] LoginRequest loginRequest)
    {
        LoginResult result = await _authManager.GenerateTokenIfCorrectCredentials(loginRequest);
        return Ok(result);
    }

    [HttpPost(UserEndpoints.TokenLogin)]
    [Authorize(Roles = nameof(RoleTypes.Normal))] // token still valid but user got deleted when db dropped
    public async Task<IActionResult> GetUserDto() // bad cos parametres often get cached and-or expoised
    {
        Guid userId = new Guid(HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var userDto = await _userRepository.MapUserDto(userId);

        LoginResult result = new LoginResult()
        {
            Token = "",
            UserDto = userDto,

        };
        return Ok(result);
    }

    [HttpPost(UserEndpoints.Register)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest registerRequest)
    {
        await _authManager.TryRegister(registerRequest);
        return Ok();
    }

    [HttpGet("test")]
    [Authorize(Roles = nameof(RoleTypes.Admin))]
    public async Task<IActionResult> AuthorizedTest(string token) // just for test
    {
        return Ok();
    }
}
