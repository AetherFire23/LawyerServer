using JWTAuth.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Models;
public class FailedRegisterRequest : IMessageResult
{
    public string Message { get; set; } = string.Empty;

}
