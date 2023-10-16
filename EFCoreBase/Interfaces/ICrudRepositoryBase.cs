using Microsoft.EntityFrameworkCore;
using EFCoreBase.Entities;

namespace EFCoreBase.Interfaces;

// necessary so that multiple inheritante of interfaces works to access the services methods

public interface ICrudRepositoryBase<T1, T2> 
    where T1 : DbContext
    where T2 : EntityBase
{

    Task Add(T2 entity);
    Task AddRange(List<T2> entity);
    Task<T2> GetEntityById(Guid id);
    Task Remove(Guid id);
    Task Remove(T2 entity);
}
