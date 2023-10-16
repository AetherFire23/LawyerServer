using EFCoreBase.Entities;

namespace ProcedureMakerServer.Authentication;

public class User : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;

    // email and shit
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public List<Role> Roles => UserRoles.Select(ur => ur.Role).ToList();
}

public class Role : EntityBase
{
    public RoleTypes RoleType { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
}

public class UserRole : EntityBase
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}