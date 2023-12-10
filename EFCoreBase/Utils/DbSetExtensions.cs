using System.Linq.Expressions;
using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBase.Utils;
public static class DbSetExtensions
{
    public static async Task RemoveAll<T>(this DbSet<T> self, Expression<Func<T, bool>> predicate) where T : EntityBase
    {
        var entities = await self.Where(predicate).ToListAsync();
        self.RemoveRange(entities);
    }

    public static Task<T> FirstByIdAsync<T>(this DbSet<T> self, Guid id) where T : EntityBase
    {
        var entity = self.FirstAsync(x => x.Id == id);
        return entity;
    }
    
    public static Task<T> FirstByIdAsync<T>(this DbSet<T> self, T other) where T : EntityBase
    {
        var entity = self.FirstAsync(x => x.Id == other.Id);
        return entity;
    }
}
