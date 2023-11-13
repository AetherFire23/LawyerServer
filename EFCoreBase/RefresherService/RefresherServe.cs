using EFCoreBase.Entities;
using EFCoreBase.Interfaces;
using EFCoreBase.Utils;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBase.RefresherService;
public interface IRefresherServe
{
    Task RefreshEntities<T>(IEnumerable<T> upToDateElements, IEnumerable<T> currentTrackedElements) where T : EntityBase;
}

public class RefresherServe<TContext> : IRefresherServe
    where TContext : DbContext
{
    private readonly TContext _context;

    public RefresherServe(TContext context)
    {
        _context = context;
    }

    public async Task RefreshEntities<T>(IEnumerable<T> upToDateElements, IEnumerable<T> currentTrackedElements)
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
    }


    public async Task RefreshAndUpdateEntities<T>(IEnumerable<T> upToDateElements,
        IEnumerable<T> currentTrackedElements,
        Action<T, T> updateFunction)
        where T : EntityBase
    {
        await RefreshEntities(upToDateElements, currentTrackedElements);
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
            var upToDateEntity = upToDateElements.First(x => x.Id == currentTrackedEntity.Id);
            updateFunction?.Invoke(currentTrackedEntity, upToDateEntity);
        }
    }
}
