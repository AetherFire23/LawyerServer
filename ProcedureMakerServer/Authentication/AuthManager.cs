using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Authentication.ReturnModels;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Exceptions.HttpResponseExceptions;
using Crypt = BCrypt.Net.BCrypt;
namespace ProcedureMakerServer.Authentication;


public class AuthManager : IAuthManager
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenManager _jwtTokenManager;
    private readonly ProcedureContext _context;

    public AuthManager(IUserRepository userRepository,
                       IJwtTokenManager jwtTokenManager,
                       ProcedureContext context)
    {
        _userRepository = userRepository;
        _jwtTokenManager = jwtTokenManager;
        _context = context;
    }

    // should rechange everything to the atualy datatypes i wanna send I guess
    public async Task<LoginResult> GenerateTokenIfCorrectCredentials(LoginRequest loginRequest)
    {
        User? user = await _userRepository.GetUserByName(loginRequest.Username);

        if (user is null) throw new InvalidCredentialsException();
        if (!Crypt.Verify(loginRequest.Password, user.HashedPassword)) throw new InvalidCredentialsException();

        // just fire exceptions and handle it in useexcetionHandler
        // middle for catching exceptions

        string token = await _jwtTokenManager.GenerateToken(user);
        UserDto userDto = await _userRepository.MapUserDto(user.Id);

        LoginResult req = new LoginResult()
        {
            Token = token,
            UserDto = userDto,
        };

        return req;
    }

    public async Task TryRegister(RegisterRequest registerRequest)
    {
        User? testedUser = await _userRepository.GetUserByName(registerRequest.Username);
        bool isUserExists = testedUser is not null;

        if (isUserExists) throw new InvalidCredentialsException();

        string hashedPassword = Crypt.HashPassword(registerRequest.Password);

        User user = new()
        {
            HashedPassword = hashedPassword,
            Name = registerRequest.Username,
        };

        Role role = new()
        {
            RoleType = registerRequest.Role,
        };

        UserRole userRole = new()
        {
            Role = role,
            User = user,
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        Lawyer lawyer = new()
        {
            User = user,
            FirstName = user.Name,
        };

        await _context.Lawyers.AddAsync(lawyer);
        await _context.SaveChangesAsync();

        // default billing element here
        var defaultBillingElementAtRegistration = new BillingElement
        {
            ActivityName = user.Name,
            Amount = 100,
            ManagerLawyer = lawyer,
            IsHourlyRate = true,
        };

        _context.BillingElements.Add(defaultBillingElementAtRegistration);
        await _context.SaveChangesAsync();

        lawyer.DefaultHourlyElement = defaultBillingElementAtRegistration;
        await _context.SaveChangesAsync();

        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();

        await _context.UserRoles.AddAsync(userRole);
        await _context.SaveChangesAsync();
    }
}
