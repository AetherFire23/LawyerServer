using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Authentication.ReturnModels;



[TsClass]
public class RegisterResult
{
    public UserDto User { get; set; }

    public RegisterResult(UserDto user)
    {
        User = user;
    }
}
