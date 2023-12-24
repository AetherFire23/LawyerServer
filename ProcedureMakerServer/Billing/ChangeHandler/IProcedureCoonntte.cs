using EFCoreBase.Entities;
using EFCoreBase.Utils;
using Microsoft.EntityFrameworkCore;

namespace ProcedureMakerServer.Billing.ChangeHandler;

public interface IContextReference
{
    public ProcedureContext ProcedureContext { get; set; }
}


// commence a etre sexy cette class-la
public static class IContextReferenceExtensions
{
    public static async Task HandleChange<T>(this IContextReference self,
        IEnumerable<T> updatedEntities,
        IEnumerable<T> storedEntities,
        Func<IEnumerable<T>, IEnumerable<T>, T, T, Task> onUpdate)
    where T : EntityBase
    {
        (IEnumerable<T> removed, IEnumerable<T> updated) = EntitiesRefesher.GetRefreshResultGeneric<T>(updatedEntities, storedEntities);

        DbSet<T> set = self.ProcedureContext.Set<T>();
        foreach (T rem in removed)
        {
            T entity = await set.FirstAsync(x => x.Id == rem.Id);
            _ = set.Remove(entity);
        }
        foreach (T upd in updated)
        {
            T entity = await set.FirstAsync(x => x.Id == upd.Id);
            await onUpdate(updatedEntities, storedEntities, upd, entity);
            _ = await self.ProcedureContext.SaveChangesAsync();
        }
    }

    public static async Task HandleChange<T>(this IContextReference self,
        IEnumerable<T> updatedEntities,
        IEnumerable<T> storedEntities,
        Func<T, T, Task> onUpdate)
        where T : EntityBase
    {
        (IEnumerable<T> removed, IEnumerable<T> updated) = EntitiesRefesher.GetRefreshResultGeneric<T>(updatedEntities, storedEntities);

        DbSet<T> set = self.ProcedureContext.Set<T>();

        foreach (T rem in removed)
        {
            T entity = await set.FirstAsync(x => x.Id == rem.Id);
            _ = set.Remove(entity);
        }

        foreach (T upd in updated)
        {
            T entity = await set.FirstAsync(x => x.Id == upd.Id);
            await onUpdate(upd, entity);
            _ = await self.ProcedureContext.SaveChangesAsync();
        }
    }



    // T1 should be updated
    // therefore t2 is set
    public static async Task HandleChangeDto<T, T2>(this IContextReference self,
    IEnumerable<T> updatedEntities,
    IEnumerable<T2> storedEntities,
    Func<IEnumerable<T>, IEnumerable<T2>, T, T2, Task> onUpdate)
        where T : EntityBase
        where T2 : EntityBase
    {
        (IEnumerable<T2> removed, IEnumerable<T> updated) = EntitiesRefesher.GetRefreshResultGeneric(updatedEntities, storedEntities);

        DbSet<T2> set = self.ProcedureContext.Set<T2>();
        foreach (T2 rem in removed)
        {
            T2 entity = await set.FirstAsync(x => x.Id == rem.Id);
            _ = set.Remove(entity);
        }
        foreach (T upd in updated)
        {
            T2 entity = await set.FirstAsync(x => x.Id == upd.Id);
            await onUpdate(updatedEntities, storedEntities, upd, entity);
            _ = await self.ProcedureContext.SaveChangesAsync();
        }
    }

    // T1 should be updated, therefore t2 is set
    public static async Task HandleChangeDto<T, T2>(this IContextReference self,
        IEnumerable<T> updatedEntities,
        IEnumerable<T2> storedEntities,
        Func<T, T2, Task> onUpdate)
        where T : EntityBase
        where T2 : EntityBase
    {
        (IEnumerable<T2> removed, IEnumerable<T> updated) = EntitiesRefesher.GetRefreshResultGeneric(updatedEntities, storedEntities);

        DbSet<T2> set = self.ProcedureContext.Set<T2>();

        foreach (T2 rem in removed)
        {
            T2 entity = await set.FirstAsync(x => x.Id == rem.Id);
            _ = set.Remove(entity);
        }

        foreach (T upd in updated)
        {
            T2 entity = await set.FirstAsync(x => x.Id == upd.Id);
            await onUpdate(upd, entity);
            _ = await self.ProcedureContext.SaveChangesAsync();
        }
    }
}