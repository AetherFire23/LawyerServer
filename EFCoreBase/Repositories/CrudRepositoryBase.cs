using EFCoreBase.Entities;
using EFCoreBase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBase.Repositories;

public abstract class CrudRepositoryBase<TContext, TEntity> : EntityRepositoryBase<TContext, TEntity>, ICrudRepositoryBase<TContext, TEntity>
    where TContext : DbContext
    where TEntity : EntityBase
{
    protected CrudRepositoryBase(TContext context) : base(context)
    {

    }

    public virtual async Task<TEntity> GetEntityById(Guid id)
    {
        var entity = await Set.FirstOrDefaultAsync(x => x.Id == id);
        return entity;
    }

    public async Task Create(TEntity entity)
    {
        await Set.AddAsync(entity);
    }

    public async Task CreateRange(List<TEntity> entity)
    {
        await Set.AddRangeAsync(entity);
    }

    public async Task Remove(Guid id)
    {
        var entity = await GetEntityById(id);
        Set.Remove(entity);
    }

    public async Task Remove(TEntity entity)
    {
        Set.Remove(entity);
    }
}
