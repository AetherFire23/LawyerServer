using EFCoreBase.Entities;
using ProcedureMakerServer.Authentication;


namespace ProcedureShared.Authentication;


// make UserContext instead and put Lawyer 1 more up

public class UserDto : EntityBase
{
	public string Name { get; set; } = string.Empty;
	public Guid LawyerId { get; set; }
	public List<RoleTypes> Roles { get; set; } = new List<RoleTypes>();
}
