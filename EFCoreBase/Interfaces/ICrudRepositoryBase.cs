using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBase.Interfaces;

// necessary so that multiple inheritante of interfaces works to access the services methods

public interface ICrudRepositoryBase<T1, T2>
    where T1 : DbContext
    where T2 : EntityBase
{

    Task Create(T2 entity);
    Task CreateRange(List<T2> entity);
    Task<T2> GetEntityById(Guid id);
    Task Remove(Guid id);
    Task Remove(T2 entity);
}
