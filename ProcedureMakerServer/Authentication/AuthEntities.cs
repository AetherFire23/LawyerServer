﻿using EFCoreBase.Entities;
using ProcedureMakerServer.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProcedureMakerServer.Authentication;

public class User : EntityBase
{
    public string Name { get; set; } = string.Empty;

    public string HashedPassword { get; set; } = string.Empty;

    public Guid LawyerId { get; set; }
    public Lawyer Lawyer { get; set; } = new Lawyer();

    public virtual ICollection<UserRole> UserRoles { get; set; }

    [NotMapped]
    public List<RoleTypes> Roles => UserRoles is null || !UserRoles.Any() ? new List<RoleTypes>() : UserRoles.Select(ur => ur.Role.RoleType).ToList();
}

public class Role : EntityBase
{
    public RoleTypes RoleType { get; set; }

    [JsonIgnore]
    public virtual ICollection<UserRole> UserRoles { get; set; }
}

public class UserRole : EntityBase
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = new Role();

    public Guid UserId { get; set; }
    public User User { get; set; } = new User();
}