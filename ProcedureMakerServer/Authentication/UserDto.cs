using EFCoreBase.Entities;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Authentication;


// make UserContext instead and put Lawyer 1 more up
[TsClass]
public class UserDto : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public Guid LawyerId { get; set; }
    public List<RoleTypes> Roles { get; set; } = new List<RoleTypes>();
}
