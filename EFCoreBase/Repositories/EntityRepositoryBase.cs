using EFCoreBase.Entities;
using EFCoreBase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBase.Repositories;
public class EntityRepositoryBase<TContext, TEntity> : RepositoryBase<TContext>
    where TContext : DbContext
    where TEntity : EntityBase
{
    public DbSet<TEntity> Set => Context.Set<TEntity>();
    public EntityRepositoryBase(TContext context) : base(context)
    {
    }
}
