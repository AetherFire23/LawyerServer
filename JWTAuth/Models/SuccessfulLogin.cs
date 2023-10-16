using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Models;
public class SuccessfulLogin<TUser>
{
    public string Token { get; set; }
    public TUser User { get; set; }
}
