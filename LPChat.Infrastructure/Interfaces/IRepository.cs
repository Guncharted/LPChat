using LPChat.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Interfaces
{
	public interface IRepository<T> where T : class, IEntity
    {
        Task CreateAsync(T item);
        Task<T> FindById(Guid id);
        Task<IEnumerable<T>> GetAsync();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task RemoveAsync(T item);
        Task<long> UpdateAsync(T item);
    }
}
