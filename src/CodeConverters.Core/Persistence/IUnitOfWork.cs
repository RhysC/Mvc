using System;

namespace CodeConverters.Core.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        bool NoTracking { get; set; }
        void Commit();
    }
}