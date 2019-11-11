using LPChat.Infrastructure.ViewModels;
using LPChat.Domain.Entities;
using LPChat.Domain.Exceptions;
using LPChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Services
{
	public class PersonInfoService : IPersonInfoService
	{
		private readonly IRepositoryManager _repoManager;

		public PersonInfoService(IRepositoryManager repoManager)
		{
			_repoManager = repoManager;
		}

		public async Task<PersonInfoViewModel> GetOneAsync(Guid personId)
		{
			var repository = _repoManager.GetRepository<Person>();
			var person = await repository.FindById(personId);
			
			if (person == null)
			{
				throw new PersonNotFoundException($"Person with id {personId} does not exist!");
			}

			var personInfo = MapToPersonInfo(person);

			return personInfo;
		}

		public async Task<IEnumerable<PersonInfoViewModel>> GetManyAsync(IEnumerable<Guid> IDs)
		{
			var repository = _repoManager.GetRepository<Person>();
			var persons = await repository.GetAsync(p => IDs.Contains(p.ID));
			var personsInfo = persons.Select(p => MapToPersonInfo(p));

			return personsInfo;
		}

		public string GetPersonDisplayName(PersonInfoViewModel personInfo)
		{
			if (string.IsNullOrWhiteSpace(personInfo.FirstName) || string.IsNullOrWhiteSpace(personInfo.LastName))
			{
				return personInfo.Username;
			}

			return string.Format($"{personInfo.FirstName} {personInfo.LastName}");
		}

		private PersonInfoViewModel MapToPersonInfo(Person person)
		{
			var personInfo = new PersonInfoViewModel
			{
				ID = person.ID,
				Username = person.Username,
				FirstName = person.FirstName,
				LastName = person.LastName
			};

			return personInfo;
		}

	}
}
