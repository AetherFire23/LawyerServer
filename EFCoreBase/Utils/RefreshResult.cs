using EFCoreBase.Entities;

namespace EFCoreBase.Utils;

public static class EntitiesRefesher
{
    public static (IEnumerable<EntityBase> Removed, IEnumerable<EntityBase> Updated) GetRefreshResult(IEnumerable<EntityBase> upToDate, IEnumerable<EntityBase> old)
    {
        IEnumerable<EntityBase> disappeared = old.Where(x => !upToDate.Any(y => y.Id == x.Id));
        IEnumerable<EntityBase> commonTrackedElements = old.Where(x => upToDate.Any(y => y.Id == x.Id));

        if (!old.Any())
        {
            disappeared = new List<EntityBase>();
            commonTrackedElements = new List<EntityBase>();
        }
        else if (!upToDate.Any())
        {
            disappeared = old;
            commonTrackedElements = new List<EntityBase>();
        }
        else
        {
            disappeared = old.Where(x => !upToDate.Any(y => y.Id == x.Id));
            commonTrackedElements = old.Where(x => upToDate.Any(y => y.Id == x.Id));
        }
        (IEnumerable<EntityBase> disappeared, IEnumerable<EntityBase> commonTrackedElements) result = (disappeared, commonTrackedElements);
        return result;
    }


    public static (IEnumerable<T> Removed, IEnumerable<T> Updated) GetRefreshResultGeneric<T>(IEnumerable<T> upToDate, IEnumerable<T> old)
        where T : EntityBase
    {
        IEnumerable<T> disappeared = old.Where(x => !upToDate.Any(y => y.Id == x.Id));
        IEnumerable<T> commonTrackedElements = old.Where(x => upToDate.Any(y => y.Id == x.Id));

        if (!old.Any())
        {
            disappeared = new List<T>();
            commonTrackedElements = new List<T>();
        }
        else if (!upToDate.Any())
        {
            disappeared = old;
            commonTrackedElements = new List<T>();
        }
        else
        {
            disappeared = old.Where(x => !upToDate.Any(y => y.Id == x.Id));
            commonTrackedElements = old.Where(x => upToDate.Any(y => y.Id == x.Id));
        }
        (IEnumerable<T> disappeared, IEnumerable<T> commonTrackedElements) result = (disappeared, commonTrackedElements);
        return result;
    }


    // t1 is dto
    public static (IEnumerable<TEntity> Removed, IEnumerable<TDto> Updated) GetRefreshResultGeneric<TDto, TEntity>(IEnumerable<TDto> upToDate, IEnumerable<TEntity> old)
    where TDto : EntityBase
    where TEntity : EntityBase
    {
        IEnumerable<TEntity> disappeared;
        IEnumerable<TDto> commonTrackedElements;

        if (!old.Any())
        {
            disappeared = new List<TEntity>();
            commonTrackedElements = new List<TDto>();
        }
        else if (!upToDate.Any())
        {
            disappeared = old;
            commonTrackedElements = new List<TDto>();
        }
        else
        {
            disappeared = old.Where(x => !upToDate.Any(y => y.Id == x.Id));
            commonTrackedElements = upToDate.Where(x => old.Any(y => y.Id == x.Id));
        }
        (IEnumerable<TEntity> disappeared, IEnumerable<TDto> commonTrackedElements) result = (disappeared, commonTrackedElements);
        return result;
    }
}