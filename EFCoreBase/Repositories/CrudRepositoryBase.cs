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
		TEntity entity = await Set.FirstAsync(x => x.Id == id);
		return entity;
	}

	public async Task CreateLawyer(TEntity entity)
	{
		_ = await Set.AddAsync(entity);
	}

	public async Task CreateRange(List<TEntity> entity)
	{
		await Set.AddRangeAsync(entity);
	}

	public async Task Remove(Guid id)
	{
		TEntity entity = await GetEntityById(id);
		_ = Set.Remove(entity);
	}

	public async Task Remove(TEntity entity)
	{
		_ = Set.Remove(entity);
	}
}
