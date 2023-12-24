using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EFCoreBase.Utils;
public static class DbSetExtensions
{
    public static async Task RemoveAll<T>(this DbSet<T> self, Expression<Func<T, bool>> predicate) where T : EntityBase
    {
        List<T> entities = await self.Where(predicate).ToListAsync();
        self.RemoveRange(entities);
    }

    public static Task<T> FirstByIdAsync<T>(this DbSet<T> self, Guid id) where T : EntityBase
    {
        Task<T> entity = self.FirstAsync(x => x.Id == id);
        return entity;
    }

    public static Task<T> FirstByIdAsync<T>(this DbSet<T> self, T other) where T : EntityBase
    {
        Task<T> entity = self.FirstAsync(x => x.Id == other.Id);
        return entity;
    }
}
