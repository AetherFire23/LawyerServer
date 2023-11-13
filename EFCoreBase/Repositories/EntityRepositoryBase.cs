using EFCoreBase.Entities;
using EFCoreBase.Utils;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBase.Repositories;
public class EntityRepositoryBase<TContext, TEntity> : RepositoryBase<TContext>
    where TContext : DbContext
    where TEntity : EntityBase
{
    public DbSet<TEntity> Set => Context.Set<TEntity>();

    protected async Task UpdateElements<T>(IEnumerable<T> upToDateElements, IEnumerable<T> currentTrackedElements)
    where T : TEntity
    {
        var refreshResult = EntitiesRefesher.GetRefreshResult(upToDateElements, currentTrackedElements);
        foreach (var newElement in refreshResult.Appeared)
        {
            await Context.Set<T>().AddAsync(newElement);
        }

        foreach (var disappearedBillingElement in refreshResult.Disappeared)
        {
            Context.Set<T>().Remove(disappearedBillingElement);
        }
    }

    public EntityRepositoryBase(TContext context) : base(context)
    {
    }
}
