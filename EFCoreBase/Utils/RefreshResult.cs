using EFCoreBase.Entities;

namespace EFCoreBase.Utils;

public static class EntitiesRefesher
{
    public static RefreshResult<T> GetRefreshResult<T>(IEnumerable<T> upToDate, IEnumerable<T> old)
         where T : EntityBase
    {
        var appeared = upToDate.Where(x => !old.Any(y => y.Id == x.Id));
        var disappeared = old.Where(x => !upToDate.Any(y => y.Id == x.Id));
        var alreadyTracking = old.Where(x => upToDate.Any(y => y.Id == x.Id));
        var result = new RefreshResult<T>(appeared, disappeared, alreadyTracking);
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
