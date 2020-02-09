using LPChat.Common.DbContracts;

namespace LPChat.Common.DbContracts
{
	public interface IRepositoryManager
    {
        IRepository<T> GetRepository<T>() where T : class, IMongoEntity;
    }
}
