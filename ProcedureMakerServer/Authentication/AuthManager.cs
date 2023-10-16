using OneOf;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Authentication.ReturnModels;
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

        if (user is null) return new FailedLoginResult();
        if (!Crypt.Verify(loginRequest.Password, user.HashedPassword)) return new FailedLoginResult();

        string token = await _jwtTokenManager.GenerateToken(user);
        return new SuccessLoginResult()
        {
            Token = token,
            User = user,
        };
    }

    public async Task<OneOf<FailedRegisterResult, SuccessRegisterResult>> TryRegister(RegisterRequest registerRequest)
    {
        bool isUserExists = await _userRepository.GetUserByName(registerRequest.Username) is not null;
        if (isUserExists) return new FailedRegisterResult();

        User user = new User()
        {
            HashedPassword = Crypt.HashPassword(registerRequest.Password),
            Name = registerRequest.Username,
        };

        Role role = new Role()
        {
            RoleType = registerRequest.Role,
        };

        UserRole userRole = new UserRole()
        {
            RoleId = role.Id,
            UserId = user.Id,
        };

        await _context.UserRoles.AddAsync(userRole);
        await _context.Users.AddAsync(user);
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();

        return new SuccessRegisterResult(user);
    }
}
