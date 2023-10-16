using EFCoreBase.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcedureMakerServer.Authentication;

public class User : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;

    public virtual ICollection<UserRole> UserRoles { get; set; }

    [NotMapped]
    public List<Role> Roles => UserRoles is null? new List<Role>() : UserRoles.Select(ur => ur.Role).ToList();
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