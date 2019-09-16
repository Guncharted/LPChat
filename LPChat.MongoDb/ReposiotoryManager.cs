using LPChat.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPChat.MongoDb
{
    public class ReposiotoryManager : IRepositoryManager
    {
        private string _dbName;
        private string _connectionString;

        public ReposiotoryManager(string dbName, string connectionString)
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
