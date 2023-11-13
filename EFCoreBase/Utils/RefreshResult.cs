using EFCoreBase.Entities;

namespace EFCoreBase.Utils;

public static class EntitiesRefesher
{
    public static RefreshResult<T> GetRefreshResult<T>(IEnumerable<T> upToDate, IEnumerable<T> old)
         where T : EntityBase
    {
        IEnumerable<T> appeared = upToDate.Where(x => !old.Any(y => y.Id == x.Id));
        IEnumerable<T> disappeared = old.Where(x => !upToDate.Any(y => y.Id == x.Id));
        IEnumerable<T> commonTrackedElements = old.Where(x => upToDate.Any(y => y.Id == x.Id));

        if (!old.Any())
        {
            appeared = upToDate;
            disappeared = new List<T>();
            commonTrackedElements = new List<T>();
        }
        else if (!upToDate.Any())
        {
            appeared = new List<T>();
            disappeared = old;
            commonTrackedElements = new List<T>();
        }
        else
        {
            appeared = upToDate.Where(x => !old.Any(y => y.Id == x.Id));
            disappeared = old.Where(x => !upToDate.Any(y => y.Id == x.Id));
            commonTrackedElements = old.Where(x => upToDate.Any(y => y.Id == x.Id));
        }
        var result = new RefreshResult<T>(appeared, disappeared, commonTrackedElements);
        return result;
    }
}

public class RefreshResult<T>
{
    public IEnumerable<T> Appeared;
    public IEnumerable<T> Disappeared;
    public IEnumerable<T> AlreadyTrackedEntities;

    public RefreshResult(IEnumerable<T> appeared, IEnumerable<T> disappeared, IEnumerable<T> alreadyTrackedEntities)
    {
        Appeared = appeared;
        Disappeared = disappeared;
        AlreadyTrackedEntities = alreadyTrackedEntities;
    }
}
