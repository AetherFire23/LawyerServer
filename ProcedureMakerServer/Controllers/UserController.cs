using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Authentication.ReturnModels;
using ProcedureMakerServer.Constants;
using ProcedureMakerServer.Scratches;
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

    [DefaultReturnType(typeof(LoginResult))]
    [HttpPut(UserEndpoints.CredentialsLogin)]
    public async Task<IActionResult> CredentialsLoginRequest([FromBody] LoginRequest loginRequest)
    {
        Console.WriteLine($"token login request procced{loginRequest.Username}");
        LoginResult result = await _authManager.GenerateTokenIfCorrectCredentials(loginRequest);
        return Ok(result);
    }

    [HttpPost(UserEndpoints.TokenLogin)]
    [DefaultReturnType(typeof(LoginResult))]
    [Authorize(Roles = nameof(RoleTypes.Admin))] // token still valid but user got deleted when db dropped
    public async Task<IActionResult> TokenLogin() // bad cos parametres often get cached and-or expoised
    {
        // work in progress 
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


    //[DefaultReturnType(typeof(void))]
    //[HttpPost("LolzidaEndpoint")]
    //public async Task<IActionResult> RegisterUser(string lolzida, string otherParam)
    //{
    //    return Ok();
    //}



    [HttpGet("authorizedrequest")]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<IActionResult> AuthorizedTesst() // just for test
    {
        return Ok();
    }



    //[HttpGet("test2")]
    //public async Task<IActionResult> test(string token, string token2) // just for test
    //{
    //    return Ok();
    //}

    //public static int Counter = 0;
    //[HttpGet("getcount")]
    //public async Task<IActionResult> Test() // just for test
    //{
    //    Console.WriteLine($"Get {Counter}");
    //    return Ok(Counter);
    //}

    //[HttpPost("setcount")]
    //public async Task<IActionResult> Test(int count) // just for test
    //{
    //    Counter = count;
    //    Console.WriteLine($"Set${Counter}");
    //    return Ok(new {Counter = count});
    //}
}
