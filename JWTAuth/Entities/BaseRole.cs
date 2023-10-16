using EFCoreBase.Entities;
using JWTAuth.Interfaces;
using System.Transactions;

public abstract class BaseRole<TRoleDeclarationBase, TRole, TUserRole, TUser, TRoleType> : EntityBase
    where TRole : BaseRole<TRoleDeclarationBase, TRole, TUserRole, TUser, TRoleType>
    where TRoleDeclarationBase : RoleDeclarationBase<TRoleDeclarationBase, TUser, TUserRole, TRole, TRoleType>
    where TUserRole : BaseUserRole<TRoleDeclarationBase, TUserRole, TUser, TRole, TRoleType>
    where TUser : BaseUser<TRoleDeclarationBase, TUser, TUserRole, TRole, TRoleType>
    where TRoleType : struct, Enum 
{
    //public Guid Id { get; set; }
    public TRoleType RoleType { get; set; }
    public ICollection<TUserRole> UserRoles { get; set; }
}