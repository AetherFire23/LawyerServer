using OneOf;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Authentication.ReturnModels;
using ProcedureMakerServer.Entities;
using Crypt = BCrypt.Net.BCrypt;
namespace ProcedureMakerServer.Authentication;


public class AuthManager : IAuthManager
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenManager _jwtTokenManager;
    private readonly ProcedureContext _context;

    public AuthManager(IUserRepository userRepository, IJwtTokenManager jwtTokenManager, ProcedureContext context)
    {
        _userRepository = userRepository;
        _jwtTokenManager = jwtTokenManager;
        _context = context;
    }

    public async Task<OneOf<FailedLoginResult, SuccessLoginResult>> GenerateTokenIfCorrectCredentials(LoginRequest loginRequest)
    {
        var user = await _userRepository.GetUserByName(loginRequest.Username);

        if (user is null) return new FailedLoginResult("incorrect userName");
        if (!Crypt.Verify(loginRequest.Password, user.HashedPassword)) return new FailedLoginResult("inccorect password");

        string token = await _jwtTokenManager.GenerateToken(user);
        UserDto userDto = await _userRepository.MapUserDto(user.Id);

        return new SuccessLoginResult()
        {
            Token = token,
            UserDto = userDto,
        };
    }

    public async Task<OneOf<FailedRegisterResult, SuccessRegisterResult>> TryRegister(RegisterRequest registerRequest)
    {
        var testedUser = await _userRepository.GetUserByName(registerRequest.Username);
        bool isUserExists = testedUser is not null;
        if (isUserExists) return new FailedRegisterResult("user exist");

        string hashedPassword = Crypt.HashPassword(registerRequest.Password);
        User user = new User()
        {
            HashedPassword = hashedPassword,
            Name = registerRequest.Username,
        };

        Role role = new Role()
        {
            RoleType = registerRequest.Role,
        };

        UserRole userRole = new UserRole()
        {
            Role = role,
            User = user,
        };

        var lawyer = new Lawyer()
        {
            UserId = user.Id,
            FirstName = user.Name,
        };

        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();

        await _context.UserRoles.AddAsync(userRole);

        await _context.Lawyers.AddAsync(lawyer);
        await _context.SaveChangesAsync();

        var userDto = await _userRepository.MapUserDto(user.Id);
        return new SuccessRegisterResult(userDto);
    }
}
