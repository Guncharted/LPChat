using LPChat.Domain.Exceptions;
using LPChat.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LPChat.MongoDb
{
	public class MongoDbRepository<T> : IRepository<T>
		where T : class, IEntity
	{
		public IMongoCollection<T> MongoCollection { get; }

		public MongoDbRepository(string dbName, string collectionName, string dbUrl)
		{
			var mongoClient = new MongoClient(dbUrl);
			var mongoDatabase = mongoClient.GetDatabase(dbName);

			MongoCollection = mongoDatabase.GetCollection<T>(collectionName);
		}

		public async Task CreateAsync(T item)
		{
			await MongoCollection.InsertOneAsync(item);
		}

		public async Task<T> FindById(Guid id)
		{
			var dbResult = await MongoCollection.FindAsync(i => i.ID == id);
			var item = dbResult.ToList().FirstOrDefault();
			return item;
		}

		public async Task<IEnumerable<T>> GetAsync()
		{
			var dbList = await MongoCollection.FindAsync(new BsonDocument());
			var resultList = dbList.ToList();
			return resultList;
		}

		public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
		{
			var resultList = new List<T>();
			var dbList = await MongoCollection.FindAsync(predicate);

			resultList = dbList.ToList();
			return resultList;
		}

		public async Task RemoveAsync(T item)
		{
			var deleteResult = await MongoCollection.DeleteOneAsync(i => i.ID == item.ID);

			if (!deleteResult.IsAcknowledged)
				throw new BsonException("Failed to remove entry.");
		}

		public async Task<long> UpdateAsync(T item)
		{
			//TODO.Replace with GUARD
			if (item == null)
				throw new ArgumentNullException(nameof(item));

			try
			{
				var oldDate = item.LastUpdatedUtcDate;
				item.LastUpdatedUtcDate = DateTime.UtcNow;
				var result = await MongoCollection.ReplaceOneAsync<T>(i => i.ID == item.ID && i.LastUpdatedUtcDate == oldDate, item);

				return result.IsAcknowledged ? result.ModifiedCount : 0;
			}
			catch (Exception ex)
			{
				throw new DatabaseException("Database exception occurred", ex);
			}

		}
	}
}
