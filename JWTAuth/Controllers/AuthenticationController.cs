//using JWTAuth.Interfaces;
//using JWTAuth.Models;
//using JWTAuth.Services;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WebAPI.Authentication;

//namespace JWTAuth.Controllers;
//[ApiController]
//[Route("[controller]")]
//public abstract class AuthenticationController<TRoleDeclaration, TAuthenticationService, TAuthenticationDb, TUser, TRole, TUserRole, TRoleType, TRegisterRequest,
//    TRegisterRequest, TJWTokenManager, TSuccessfulLoginRequest> : ControllerBase
//    where TRoleDeclaration : RoleDeclarationBase<TRoleDeclaration, TUser, TUserRole, TRoleDeclaration, TRoleType>
//    where TAuthenticationDb : IAuthDb2<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
//    where TAuthenticationService : AuthenticationServiceBase<TRoleDeclaration, TAuthenticationDb, TUser, TUserRole, TRole, TRoleType, TRegisterRequest, >
//    where TUser : BaseUser<TRoleDeclaration, TRole, TUserRole, TUser, TRoleType>
//    where TRole : BaseRole<TRoleDeclaration, TRole, TUserRole, TUser, TRoleType>
//    where TUserRole : BaseUserRole<TRoleDeclaration, TUserRole, TUser, TRole, TRoleType>
//    where TRoleType : struct, Enum
//    where TRegisterRequest : RegisterRequestBase<TRoleType>
//    where TJWTokenManager : JwtTokenManagerBase<TRoleDeclaration, TAuthenticationDb, TUser, TUserRole, TRole, TRoleType, TRegisterRequest>


//{
//    public AuthenticationController()
//    {

//    }

//    public async Task<IActionResult> TryLogin(LoginRequest loginRequest)
//    {

//    }
//}
