using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Scratches;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace ProcedureMakerServer.Controllers;

[ApiController]
[Route("[controller]")]
//[ServiceFilter(typeof(HttpResponseExceptionFilter))]
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

    [HttpPut("token")]
    public async Task<IActionResult> GenerateTokenIfValid([FromBody] LoginRequest loginRequest)
    {
        return (await _authManager.GenerateTokenIfCorrectCredentials(loginRequest))
            .Match<IActionResult>(Ok, BadRequest);
    }

    [HttpGet("tokenLogin")]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<IActionResult> GetUserDto() // bad cos parametres often get cached and-or expoised
    {
        Guid userId = new Guid(HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var userDto = await _userRepository.MapUserDto(userId);

        return Ok(userDto);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest registerRequest)
    {
        var result = await _authManager.TryRegister(registerRequest);
        return Ok(result);
    }

    [HttpGet("test")]
    [Authorize(Roles = nameof(RoleTypes.Admin))]
    public async Task<IActionResult> AuthorizedTest(string token) // just for test
    {
        return Ok();
    }

    [HttpGet("test7")]
    public async Task<IActionResult> BreakTest()
    {
        var obj = new LoginRequest() { Password = "dw", Username = "wd" };
        int failOrSuccess = Random.Shared.Next(0, 2);
        if (failOrSuccess == 0)
        {
            //  throw new HttpRequestException("Lolzida", new Exception(), System.Net.HttpStatusCode.NotFound);
            return NotFound(obj);

        }
        else
        {
            return Ok(obj);
        }

    }

    [HttpGet("test8")]
    public async Task<IActionResult> BreakTest2()
    {
        throw new MyInvalidException();
         
        return Ok();
    }
}