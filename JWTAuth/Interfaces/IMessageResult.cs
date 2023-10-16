using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Interfaces;
public interface IMessageResult
{
    public string Message { get; set; }
}
