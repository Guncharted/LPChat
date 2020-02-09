using LPChat.Common.DbContracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LPChat.Common.DbContracts
{
	public interface IRepository<T> where T : class, IMongoEntity
    {
        Task CreateAsync(T item);
        Task<T> FindById(Guid id);
        Task<IEnumerable<T>> GetAsync();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task RemoveAsync(T item);
        Task<long> UpdateAsync(T item);
    }
}
