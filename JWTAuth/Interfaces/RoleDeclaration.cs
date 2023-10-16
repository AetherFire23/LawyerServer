using Microsoft.EntityFrameworkCore;

namespace JWTAuth.Interfaces;
public class RoleDeclaration
{
}

public abstract class RoleDeclarationBase
{

}

public abstract class RoleDeclarationBase<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType> : RoleDeclarationBase
       where TRoleDeclaration : RoleDeclarationBase<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
       where TRoleType : struct, Enum
       where TUser : BaseUser<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
       where TUserRole : BaseUserRole<TRoleDeclaration, TUserRole, TUser, TRole, TRoleType>
       where TRole : BaseRole<TRoleDeclaration, TRole, TUserRole, TUser, TRoleType>
{

}

public interface IAuthDb2<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
       where TRoleDeclaration : RoleDeclarationBase<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
       where TRoleType : struct, Enum
       where TUser : BaseUser<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
       where TUserRole : BaseUserRole<TRoleDeclaration, TUserRole, TUser, TRole, TRoleType>
       where TRole : BaseRole<TRoleDeclaration, TRole, TUserRole, TUser, TRoleType>
{

    public DbSet<TUser> Users { get; set; }
    public DbSet<TRole> Roles { get; set; }
    public DbSet<TUserRole> UserRoles { get; set; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}