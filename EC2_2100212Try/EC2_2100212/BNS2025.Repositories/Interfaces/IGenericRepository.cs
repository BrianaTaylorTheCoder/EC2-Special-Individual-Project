using System.Linq.Expressions;

namespace BNS2025.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            List<string>? includes = null
            );
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);
        Task<T> InsertAsync(T entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
    }
}
