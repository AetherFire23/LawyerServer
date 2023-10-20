using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Authentication;

[TsClass]
public class User : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public string HashedPassword { get; set; } = string.Empty;


    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    [NotMapped]
    public List<RoleTypes> Roles => UserRoles is null || !UserRoles.Any() ? new List<RoleTypes>() : UserRoles.Select(ur => ur.Role.RoleType).ToList();
}


[TsClass]
public class Role : EntityBase
{

    public RoleTypes RoleType { get; set; }

    [JsonIgnore]
    public virtual ICollection<UserRole> UserRoles { get; set; }
}

[TsClass]
public class UserRole : EntityBase
{
    [TsProperty(StrongType = typeof(string))]
    public Guid RoleId { get; set; }

    public Role Role { get; set; } = new Role();

    [TsProperty(StrongType = typeof(string))]
    public Guid UserId { get; set; }
    public User User { get; set; } = new User();
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
    }
}