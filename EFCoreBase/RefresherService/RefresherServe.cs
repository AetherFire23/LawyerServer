using EFCoreBase.Entities;
using EFCoreBase.Interfaces;
using EFCoreBase.Utils;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBase.RefresherService;

public interface ICopyToAble<T> where T : Entities.EntityBase
{
    public void CopyTo(T target);
}

public interface IRefresherServe
{
    Task<IEnumerable<T>> RefreshAndUpdateEntities<T>(IEnumerable<T> upToDateElements, IEnumerable<T> currentTrackedElements) where T : EntityBase;
    Task RefreshAndUpdateEntitiesWithCopyTo<T>(IEnumerable<T> upToDateElements, IEnumerable<T> currentTrackedElements) where T : EntityBase, ICopyToAble<T>;
}

public class RefresherServe<TContext> : IRefresherServe
    where TContext : DbContext
{
    private readonly TContext _context;

    public RefresherServe(TContext context)
    {
        _context = context;
    }



    /// <summary>
    /// returns currently tracked entities without updating them
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="upToDateElements"></param>
    /// <param name="currentTrackedElements"></param>
    /// <returns></returns>
    public async Task<IEnumerable<T>> RefreshAndUpdateEntities<T>(IEnumerable<T> upToDateElements,
        IEnumerable<T> currentTrackedElements)
        where T : EntityBase

    {
        var refreshResult = EntitiesRefesher.GetRefreshResult(upToDateElements, currentTrackedElements);
        foreach (var newElement in refreshResult.Appeared)
        {
            await _context.Set<T>().AddAsync(newElement);
        }

        foreach (var disappearedBillingElement in refreshResult.Disappeared)
        {
            _context.Set<T>().Remove(disappearedBillingElement);
        }
        return refreshResult.AlreadyTrackedEntities;
    }


    public async Task RefreshAndUpdateEntitiesWithCopyTo<T>(IEnumerable<T>? upToDateElements,
    IEnumerable<T>? currentTrackedElements)
    where T : EntityBase, ICopyToAble<T>

    {
        var refreshResult = EntitiesRefesher.GetRefreshResult(upToDateElements, currentTrackedElements);
        foreach (var newElement in refreshResult.Appeared)
        {
            await _context.Set<T>().AddAsync(newElement);
        }

        foreach (var disappearedBillingElement in refreshResult.Disappeared)
        {
            _context.Set<T>().Remove(disappearedBillingElement);
        }

        foreach (var currentTrackedEntity in refreshResult.AlreadyTrackedEntities)
        {
            var commonTrackedEntity = upToDateElements.FirstOrDefault(x => x.Id == currentTrackedEntity.Id);
            if (commonTrackedEntity is null) continue;

            commonTrackedEntity.CopyTo(currentTrackedEntity);
        }
    }
}
