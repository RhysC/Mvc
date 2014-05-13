using System;
using System.Data.Entity;

namespace CodeConverters.Core.Persistence
{
    public interface IDbContext : IDisposable
    {
        IDbSet<T> Set<T>() where T : class;
        int SaveChanges();
    }
}