using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Repositories
{
    public class MongoDbService<T> where T : class
    {
        public IMongoCollection<T> MongoCollection { get; }

        public MongoDbService(string dbName, string collectionName, string dbUrl)
        {
            var mongoClient = new MongoClient(dbUrl);
            var mongoDatabase = mongoClient.GetDatabase(dbName);

            MongoCollection = mongoDatabase.GetCollection<T>(collectionName);
        }

        public async Task Insert(T value) => await MongoCollection.InsertOneAsync(value);

        public async Task<List<T>> GetAllAsync()
        {
            var resultList = new List<T>();

            var dbList = await MongoCollection.FindAsync(new BsonDocument());

            resultList = dbList.ToList();
            return resultList;
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> condition)
        {
            var resultList = new List<T>();

            var dbList = await MongoCollection.FindAsync(condition);

            resultList = dbList.ToList();
            return resultList;
        }

        public async Task<UpdateResult> UpdateOne(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition)
        {
            var updateResult = await MongoCollection.UpdateOneAsync(filterDefinition, updateDefinition);

            return updateResult;
        }
    }
}
