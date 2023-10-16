using JWTAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Models;
public abstract class RegisterRequestBase<TRoleTypes>
    where TRoleTypes : struct, Enum
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<TRoleTypes> ClaimedRoles { get; set; } = new();
}
