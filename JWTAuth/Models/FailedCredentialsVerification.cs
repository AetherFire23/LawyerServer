using JWTAuth.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Models;
public class FailedCredentialsVerification : IMessageResult
{
    public string Message { get; set; } = string.Empty;

    public FailedCredentialsVerification(string message)
    {
        Message = message;

    }
}
