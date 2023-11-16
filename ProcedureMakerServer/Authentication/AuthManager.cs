using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Authentication.ReturnModels;
using ProcedureMakerServer.Billing;
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
        var user = await _userRepository.GetUserByName(loginRequest.Username);

        if (user is null) throw new InvalidCredentialsException();
        if (!Crypt.Verify(loginRequest.Password, user.HashedPassword)) throw new InvalidCredentialsException();

        // just fire exceptions and handle it in useexcetionHandler
        // middle for catching exceptions

        string token = await _jwtTokenManager.GenerateToken(user);
        UserDto userDto = await _userRepository.MapUserDto(user.Id);

        var req = new LoginResult()
        {
            Token = token,
            UserDto = userDto,
        };

        return req;
    }

    public async Task TryRegister(RegisterRequest registerRequest)
    {
        var testedUser = await _userRepository.GetUserByName(registerRequest.Username);
        bool isUserExists = testedUser is not null;

        if (isUserExists) throw new InvalidCredentialsException();

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




        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();



        var lawyer = new Lawyer()
        {
            User = user,
            FirstName = user.Name,

        };
        await _context.Lawyers.AddAsync(lawyer);
        await _context.SaveChangesAsync();


        var lawyerBillingOptions = new LawyerBillingOptions()
        {
            Lawyer = lawyer,
            
           
        };
        await _context.LawyerBillingOptions.AddAsync(lawyerBillingOptions);
        await _context.SaveChangesAsync();

        var defaultBillingElement = new BillingElement()
        {
            ActivityName ="JuridicalWork",
            Amount = 100,
            Lawyer = lawyer,
            LawyerBillingOptions = lawyerBillingOptions,
        };


        await _context.BillingElements.AddAsync(defaultBillingElement);
        await _context.SaveChangesAsync();



        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();

        await _context.UserRoles.AddAsync(userRole);
        await _context.SaveChangesAsync();

        var userDto = await _userRepository.MapUserDto(user.Id);
    }
}
