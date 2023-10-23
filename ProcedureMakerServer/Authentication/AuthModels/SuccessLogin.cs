using ProcedureMakerServer.Enums;
using ProcedureMakerServer.Interfaces;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Authentication.AuthModels;

[TsClass]
public class SuccessLogin : IRequestResult
{
    public UserDto UserDto { get; set; } = new UserDto();
    public string Token { get; set; } = string.Empty;

    public RequestResultTypes Result { get; } = RequestResultTypes.Success;
}
