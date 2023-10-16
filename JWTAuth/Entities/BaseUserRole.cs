using JWTAuth.Interfaces;

public abstract class BaseUserRole<TRoleDeclarationBase, TUserRole, TUser, TRole, TRoleType>
    where TUserRole : BaseUserRole<TRoleDeclarationBase, TUserRole, TUser, TRole, TRoleType>
    where TRoleDeclarationBase : RoleDeclarationBase<TRoleDeclarationBase, TUser, TUserRole, TRole, TRoleType>
    where TRole : BaseRole<TRoleDeclarationBase, TRole, TUserRole, TUser, TRoleType>
    where TUser : BaseUser<TRoleDeclarationBase, TUser, TUserRole, TRole, TRoleType>
    where TRoleType : struct, Enum
{
    public Guid Id { get; set; }

    public Guid RoleId { get; set; }
    public TRole Role { get; set; }

    public Guid UserId { get; set; }
    public TUser User { get; set; }
}