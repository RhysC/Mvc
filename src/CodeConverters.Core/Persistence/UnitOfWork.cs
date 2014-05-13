using System;
using System.Transactions;

namespace CodeConverters.Core.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
            NoTracking = false;
        }

        public bool NoTracking { get; set; }

        public void Commit()
        {
            if (NoTracking)
                throw new InvalidOperationException("NoTracking UOW should not have any changes to commit");

            using (var scope = new TransactionScope())
            {
                _context.SaveChanges();
                scope.Complete();
            }
        }

      public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}