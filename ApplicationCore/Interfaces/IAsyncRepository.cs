using System.Collections.Generic;
using System.Threading.Tasks;

namespace Javeriana.Core.Interfaces
{
    public interface IAsyncRepository<T>
    {
        Task<T> GetByIdAsync(long id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
