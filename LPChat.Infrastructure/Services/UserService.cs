using LPChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using LPChat.Common.DbContracts;
using LPChat.Data.MongoDb.Entities;
using LPChat.Common.Exceptions;
using LPChat.Common.Models;
using LPChat.Infrastructure.Mapping;

namespace LPChat.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repoManager;
        private readonly IMemoryCache _memoryCache;

        public UserService(IRepositoryManager repoManager, IMemoryCache memoryCache)
        {
            _repoManager = repoManager;
            _memoryCache = memoryCache;
        }

        public async Task<UserModel> GetOneAsync(Guid userId)
        {
            if (TryGetFromCache(userId, out UserModel cachedUser))
            {
                return cachedUser;
            }

            var repository = _repoManager.GetRepository<User>();
            var user = await repository.FindById(userId);

            _ = user ?? throw new PersonNotFoundException($"Person with id {userId} does not exist!");

            var userModel = DataMapper.Map<User, UserModel>(user);
            AddPersonToCache(userModel);

            return userModel;
        }

        public async Task<IEnumerable<UserModel>> GetManyAsync(IEnumerable<Guid> IDs)
        {
            var repository = _repoManager.GetRepository<User>();
            var users = await repository.GetAsync(p => IDs.Contains(p.ID));
            return DataMapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(users);
        }


        //TODO below methods should be moved to separate service
        private void AddPersonToCache(UserModel person)
        {
            _memoryCache.Set<UserModel>(person.ID, person, TimeSpan.FromMinutes(30));
        }

        private bool TryGetFromCache(Guid personId, out UserModel cachedPerson)
        {
            if (_memoryCache.TryGetValue(personId, out cachedPerson))
            {
                return true;
            }

            return false;
        }
    }
}
