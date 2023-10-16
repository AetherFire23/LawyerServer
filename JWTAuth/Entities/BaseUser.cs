using JWTAuth.Interfaces;

public abstract class BaseUser<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
    where TUser : BaseUser<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
    where TRoleDeclaration : RoleDeclarationBase<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
    where TUserRole : BaseUserRole<TRoleDeclaration, TUserRole, TUser, TRole, TRoleType>
    where TRole : BaseRole<TRoleDeclaration, TRole, TUserRole, TUser, TRoleType>
    where TRoleType : struct, Enum
{
    public Guid Id { get; set; }
    public ICollection<TUserRole> UserRoles { get; set; }
    public List<TRoleType> RoleTypes => UserRoles.Select(x => x.Role.RoleType).ToList();
    public string Name { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}