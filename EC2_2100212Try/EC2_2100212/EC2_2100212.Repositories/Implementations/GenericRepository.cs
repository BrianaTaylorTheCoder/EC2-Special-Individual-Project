
using Microsoft.EntityFrameworkCore;
using EC2_2100212.Repositories.Data;
using EC2_2100212.Repositories.Interfaces;
using System.Linq.Expressions;

namespace EC2_2100212.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BagsDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(BagsDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Get all entities with optional filtering, sorting, and eager loading
        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            List<string>? includes = null)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            // Apply filter expression if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Include related entities if specified
            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            // Apply sorting if specified
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Return as no-tracking for read-only operations
            return await query.AsNoTracking().ToListAsync();
        }

        // Get all entities without any filtering, sorting or eager loading
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        // Get a single entity by its Id
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Insert a new entity
        public async Task<T> InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Delete an entity by its Id
        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return;  // Handle entity not found

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // Update an existing entity
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity); // This is enough for EF to track changes
            await _context.SaveChangesAsync();
        }

        // Dispose of resources when done
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
