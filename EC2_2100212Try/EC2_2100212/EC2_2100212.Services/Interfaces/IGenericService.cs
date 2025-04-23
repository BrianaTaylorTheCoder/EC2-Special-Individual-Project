using System.Linq.Expressions;

namespace EC2_2100212.Services.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(
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
