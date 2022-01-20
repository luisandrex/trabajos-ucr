using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRIME_UCR.Application.Repositories
{
    // generic repository with basic CRUD operations
    public interface IGenericRepository<T, TKey> where T : class
    {
        Task<T> GetByKeyAsync(TKey key);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
        Task<T> InsertAsync(T model);
        Task DeleteAsync(TKey key);
        Task UpdateAsync(T model);
    }
}