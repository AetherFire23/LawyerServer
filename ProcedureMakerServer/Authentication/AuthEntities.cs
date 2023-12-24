using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProcedureMakerServer.Authentication;


public class User : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    [NotMapped]
    public List<RoleTypes> Roles => UserRoles is null || !UserRoles.Any() ? new List<RoleTypes>() : UserRoles.Select(ur => ur.Role.RoleType).ToList();
}



public class Role : EntityBase
{
    public RoleTypes RoleType { get; set; } = RoleTypes.Normal;

    [JsonIgnore]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}


public class UserRole : EntityBase
{
    public Guid RoleId { get; set; }

    public Role Role { get; set; }


    public Guid UserId { get; set; }
    public User User { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
    }
}