using LPChat.Domain.DTO;
using LPChat.Domain.Entities;
using LPChat.Domain.Exceptions;
using LPChat.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPChat.Infrastructure.Services
{
	public class PersonInfoService
	{
		private readonly MongoDbService<Person> _personContext;

		public PersonInfoService(MongoDbService<Person> personContext)
		{
			_personContext = personContext;
		}

		public async Task<PersonInfo> GetOneAsync(Guid personId)
		{
			var person = (await _personContext.GetAsync(p => p.ID == personId)).FirstOrDefault();
			
			if (person == null)
			{
				throw new ChatAppException($"Person with id {personId} does not exist!");
			}

			var personInfo = MapToPersonInfo(person);

			return personInfo;
		}

		public async Task<IEnumerable<PersonInfo>> GetManyAsync(IEnumerable<Guid> IDs)
		{
			var persons = await _personContext.GetAsync(p => IDs.Contains(p.ID));
			var personsInfo = persons.Select(p => MapToPersonInfo(p));

			return personsInfo;
		}

		public string GetPersonDisplayName(PersonInfo personInfo)
		{
			if (string.IsNullOrWhiteSpace(personInfo.FirstName) || string.IsNullOrWhiteSpace(personInfo.LastName))
			{
				return personInfo.Username;
			}

			return string.Format($"{personInfo.FirstName} {personInfo.LastName}");
		}

		private PersonInfo MapToPersonInfo(Person person)
		{
			var personInfo = new PersonInfo
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
