using LPChat.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LPChat.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T item);
        Task<T> FindById(Guid id);
        Task<IEnumerable<T>> GetAsync();
        Task<IEnumerable<T>> GetAsync(Func<T, bool> predicate);
        Task RemoveAsync(T item);
        Task UpdateAsync(T item);
        Task UpdateRangeAsync(IEnumerable<T> items);
    }
}
