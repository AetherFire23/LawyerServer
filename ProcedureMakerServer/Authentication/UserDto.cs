using EFCoreBase.Entities;

namespace ProcedureMakerServer.Authentication;

public class UserDto : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public List<RoleTypes> Roles { get; set; } = new List<RoleTypes>();
}
