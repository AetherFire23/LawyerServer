using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Authentication.ReturnModels;
using ProcedureMakerServer.Constants;
using ProcedureShared.Authentication;
using System.Security.Claims;

namespace ProcedureMakerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
	private readonly IAuthManager _authManager;
	private readonly UserRepository _userRepository;
	private readonly IJwtTokenManager _JwtTokenManager;

	public UserController(IAuthManager authManager,
						  UserRepository userRepository,
						  IJwtTokenManager jwtTokenManager)
	{
		_authManager = authManager;
		_userRepository = userRepository;
		_JwtTokenManager = jwtTokenManager;
	}

	[HttpPut(UserEndpoints.CredentialsLogin)]
	[ProducesResponseType(200, Type = typeof(LoginResult))]
	public async Task<ActionResult<LoginResult>> CredentialsLoginRequest([FromBody] LoginRequest loginRequest)
	{
		Console.WriteLine($"token login request procced{loginRequest.Username}");
		var result = await _authManager.GenerateTokenIfCorrectCredentials(loginRequest);
		return Ok(result);
	}

	[HttpPost(UserEndpoints.TokenLogin)]
	[Authorize(Roles = nameof(RoleTypes.Admin))] // token still valid but user got deleted when db dropped
	public async Task<IActionResult> TokenLogin() // bad cos parametres often get cached and-or expoised
	{
		// work in progress 
		Guid userId = new Guid(HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

		UserDto userDto = await _userRepository.MapUserDto(userId);

		LoginResult result = new LoginResult()
		{
			Token = "",
			UserDto = userDto,

		};

		return Ok(result);
	}

	// register, if success get the created objects through credentialsLogin
	[HttpPost("register")]
	public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest registerRequest)
	{
		await _authManager.TryRegister(registerRequest);
		return Ok();
	}

	[HttpGet("authorizedrequestblabla")]
	[Authorize(Roles = nameof(RoleTypes.Normal))]
	public async Task<ActionResult> AuthorizedTesst() // just for test
	{
		return Ok();
	}
}
