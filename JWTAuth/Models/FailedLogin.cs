using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Models;
public class FailedLogin

{
    public string Message { get; set; } = "failed login";

    public FailedLogin()
    {
        
    }
    public FailedLogin(string message)
    {
        Message = message;
    }
}
