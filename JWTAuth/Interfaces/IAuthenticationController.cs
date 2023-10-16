using Microsoft.AspNetCore.Mvc;

namespace JWTAuth.Interfaces;
public interface IAuthenticationController
{
    public Task<IActionResult> TryLogin();
    public Task<IActionResult> TryRegister();
}
