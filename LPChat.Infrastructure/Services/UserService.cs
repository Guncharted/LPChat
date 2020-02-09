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

        public async Task<UserModel> GetOneAsync(Guid personId)
        {
            if (TryFromCache(personId, out UserModel cachedPerson))
            {
                return cachedPerson;
            }

            var repository = _repoManager.GetRepository<User>();
            var person = await repository.FindById(personId);

            if (person == null)
            {
                throw new PersonNotFoundException($"Person with id {personId} does not exist!");
            }

            var personInfo = MapToPersonInfo(person);
            AddPersonToCache(personInfo);

            return personInfo;
        }

        public async Task<IEnumerable<UserModel>> GetManyAsync(IEnumerable<Guid> IDs)
        {
            var repository = _repoManager.GetRepository<User>();
            var persons = await repository.GetAsync(p => IDs.Contains(p.ID));
            var personsInfo = persons.Select(p => MapToPersonInfo(p));

            return personsInfo;
        }

        public string GetPersonDisplayName(UserModel personInfo)
        {
            if (string.IsNullOrWhiteSpace(personInfo.FirstName) || string.IsNullOrWhiteSpace(personInfo.LastName))
            {
                return personInfo.Username;
            }

            return string.Format($"{personInfo.FirstName} {personInfo.LastName}");
        }

        private UserModel MapToPersonInfo(User person)
        {
            var personInfo = new UserModel
            {
                ID = person.ID,
                Username = person.Username,
                FirstName = person.FirstName,
                LastName = person.LastName
            };

            return personInfo;
        }

        private void AddPersonToCache(UserModel person)
        {
            _memoryCache.Set<UserModel>(person.ID, person, TimeSpan.FromMinutes(30));
        }

        private bool TryFromCache(Guid personId, out UserModel cachedPerson)
        {
            if (_memoryCache.TryGetValue(personId, out cachedPerson))
            {
                return true;
            }

            return false;
        }
    }
}
