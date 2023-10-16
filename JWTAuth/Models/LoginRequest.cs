using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Models;
public class LoginRequest
{
    public string UserName { get; set; } = string.Empty;
    public string PasswordAttempt { get; set; } = string.Empty;
}
