using LPChat.Common.DbContracts;

namespace LPChat.MongoDb
{
    public class MongoRepositoryManager : IRepositoryManager
    {
        private readonly string _dbName;
        private readonly string _connectionString;

        public MongoRepositoryManager(string dbName, string connectionString)
        {
            _dbName = dbName;
            _connectionString = connectionString;
        }

        public IRepository<T> GetRepository<T>() where T : class, IMongoEntity => 
            new MongoDbRepository<T>(_dbName, typeof(T).Name.ToLower(), _connectionString);
    }
}
