using JWTAuth.Interfaces;
using JWTAuth.Models;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WebAPI.Authentication;

//default equality comparer
// 
namespace JWTAuth.Services;
public abstract class AuthenticationServiceBase<TRoleDeclaration, TAuthenticationDb, TUser,
       TUserRole, TRole, TRoleType, TRegisterRequest, TJWTokenManager, TSuccessfulLoginRequest>
       // Have sex everyday to stay healthy
       where TAuthenticationDb : IAuthDb2<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
       where TRoleDeclaration : RoleDeclarationBase<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
       where TRoleType : struct, Enum
       where TUser : BaseUser<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
       where TUserRole : BaseUserRole<TRoleDeclaration, TUserRole, TUser, TRole, TRoleType>
       where TRole : BaseRole<TRoleDeclaration, TRole, TUserRole, TUser, TRoleType>
       where TRegisterRequest : RegisterRequestBase<TRoleType>
       where TJWTokenManager : JwtTokenManagerBase<TRoleDeclaration, TAuthenticationDb, TUser, TUserRole, TRole, TRoleType, TRegisterRequest>
       where TSuccessfulLoginRequest : SuccessfulLoginResultBase, new()
{
    protected readonly TAuthenticationDb _authenticationDb;
    protected readonly TJWTokenManager _tokenManager;

    private Func<SuccessfulLoginResultBase, TSuccessfulLoginRequest> _succesfulLoginRequestBuilder;

    public AuthenticationServiceBase(TAuthenticationDb authenticationDb, TJWTokenManager tokenManager)
    {
        _authenticationDb = authenticationDb;
        _tokenManager = tokenManager;

        //mega smell

        _succesfulLoginRequestBuilder = ConfigureSuccessLoginRequest(new SuccessfulLoginResultBase()) is null
            ? DefaultSuccessLoginRequest
            : ConfigureSuccessLoginRequest;
    }

    public async virtual Task<OneOf<FailedCredentialsVerification, TUser>> VerifyCredentials(LoginRequest loginRequest)
    {
        if (string.IsNullOrEmpty(loginRequest.UserName)) return new FailedCredentialsVerification("Failed");
        if (string.IsNullOrEmpty(loginRequest.PasswordAttempt)) return new FailedCredentialsVerification("Failed");

        var user = (await _authenticationDb.Users.FirstOrDefaultAsync(x => x.Name == loginRequest.UserName));
        if (user is null) return new FailedCredentialsVerification("Failed");

        bool isCorrect = VerifyPasswordHash(loginRequest.PasswordAttempt, user.PasswordHash);
        return user;
    }

    public async Task<bool> IsUserWithSameUsernameExists(TRegisterRequest registerBase)
    {
        if (string.IsNullOrEmpty(registerBase.UserName)) return false;
        if (string.IsNullOrEmpty(registerBase.Password)) return false;

        bool isUserExists = (await _authenticationDb.Users.FirstOrDefaultAsync(x => x.Name == registerBase.UserName)) is not null;
        return isUserExists;
    }

    public async Task<OneOf<FailedRegisterRequest, TUser>> CreateUser(TRegisterRequest registerRequest)
    {
        bool canAddUser = !(await IsUserWithSameUsernameExists(registerRequest));
        if (!canAddUser) return new FailedRegisterRequest() { Message = "Failed" };

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
        var newUser = await CreateUserFromRegisterRequestThenSave(registerRequest, hashedPassword);

        return newUser;
    }

    public bool VerifyPasswordHash(string passwordAttempt, string passwordHash)
    {
        bool isCorrect = BCrypt.Net.BCrypt.Verify(passwordAttempt, passwordHash);
        return isCorrect;
    }

    public async Task<OneOf<FailedCredentialsVerification, SuccessfulLoginResultBase>> CreateTokenIfCorrectCredentials(LoginRequest loginRequest)
    {
        OneOf<FailedCredentialsVerification, TUser> failOrCorrect = await VerifyCredentials(loginRequest);

        var result = await failOrCorrect.Match<Task<OneOf<FailedCredentialsVerification, SuccessfulLoginResultBase>>>(
           async (FailedCredentialsVerification failed) => failed,
           async (TUser user) =>
           {
               string token = await _tokenManager.GenerateToken(user);
               return new SuccessfulLoginResultBase(token, "SucessLogin! hahahahah");
           });

        return result;
    }

    protected abstract Task<TUser> CreateUserFromRegisterRequestThenSave(TRegisterRequest registerRequest, string hashedPassword);

    protected async Task<List<TRole>> GetRoles(List<TRoleType> roleTypes)
    {
        List<TRole> roles = new();
        foreach (TRoleType roleType in roleTypes)
        {
            //giga cahd pupper
            // EqualityComparer<TRoleType>.Default.Equals()
            // Enum does inherit from IEqualityComparer
            var role = await _authenticationDb.Roles.FirstOrDefaultAsync(x => x.RoleType.Equals(roleType));
            roles.Add(role);
        }
        return roles;
    }

    // will prolly have to genericize all login requests but whatevs
    protected async Task<OneOf<FailedCredentialsVerification, TSuccessfulLoginRequest>> TryLogin(LoginRequest loginRequest)
    {
        OneOf<FailedCredentialsVerification, SuccessfulLoginResultBase> failedOrSuccess = await CreateTokenIfCorrectCredentials(loginRequest);

        var result = failedOrSuccess.Match<OneOf<FailedCredentialsVerification, TSuccessfulLoginRequest>>(
            failed => failed,
            success =>
            {
                var successResult = _succesfulLoginRequestBuilder(success);
                return successResult;
            });
        return result;

        // end-user can now just failedOrSuccess.Match(Ok(failed) Ok(success))
    }

    private TSuccessfulLoginRequest DefaultSuccessLoginRequest(SuccessfulLoginResultBase success)
    {
        var login = new TSuccessfulLoginRequest()
        {
            Message = success.Message,
            Token = success.Token,
        };
        return login;
    }

    protected virtual TSuccessfulLoginRequest ConfigureSuccessLoginRequest(SuccessfulLoginResultBase success) { return null; }
}
