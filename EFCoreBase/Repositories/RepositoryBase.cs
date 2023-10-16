using Microsoft.EntityFrameworkCore;

namespace EFCoreBase.Repositories;
public class RepositoryBase<TContext> where TContext : DbContext
{
    public readonly TContext Context;
    public RepositoryBase(TContext context)
    {
        Context = context;
    }
}
