using LPChat.Domain.Interfaces;

namespace LPChat.Infrastructure.Interfaces
{
	public interface IRepositoryManager
    {
        IRepository<T> GetRepository<T>() where T : class, IEntity;
    }
}
