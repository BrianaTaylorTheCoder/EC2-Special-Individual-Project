using BNS2025.Repositories.Data;
using BNS2025.Repositories.Interfaces;


namespace BNS2025.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BnsDbContext _context;

        public UnitOfWork(BnsDbContext context)
        {
            _context = context;
        }
        
        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            IGenericRepository<T> repo = new GenericRepository<T>(_context);
            return repo;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }




        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

    }
}
