using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Models;
public class SuccessfulLoginResultBase
{
    public string Token { get; set; } = string.Empty;
    public string Message { get; set; } = "Successful login!";

    public SuccessfulLoginResultBase()
    {
        
    }
    public SuccessfulLoginResultBase(string token, string message)
    {
        Token = token;
        Message = message;
    }

    public SuccessfulLoginResultBase(SuccessfulLoginResultBase copy)
    {
        Token = copy.Token;
        Message = copy.Message;
    }
}
