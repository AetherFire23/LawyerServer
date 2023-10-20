using EFCoreBase.Entities;
using ProcedureMakerServer.Entities;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Authentication;

[TsClass]
public class UserDto : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public Lawyer Lawyer { get; set; }
    public List<RoleTypes> Roles { get; set; } = new List<RoleTypes>();
}
