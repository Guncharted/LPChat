using LPChat.Domain.Interfaces;
using LPChat.Infrastructure.Interfaces;

namespace LPChat.MongoDb
{
	public class MongoReposiotoryManager : IRepositoryManager
    {
        private readonly string _dbName;
        private readonly string _connectionString;

        public MongoReposiotoryManager(string dbName, string connectionString)
        {
            _dbName = dbName;
            _connectionString = connectionString;
        }

        public IRepository<T> GetRepository<T>() where T : class, IEntity
        {
            return new MongoDbRepository<T>(_dbName, typeof(T).Name.ToLower(), _connectionString);
        }
    }
}
