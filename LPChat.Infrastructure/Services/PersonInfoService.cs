using LPChat.Core.DTO;
using LPChat.Core.Entities;
using LPChat.Core.Exceptions;
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

		public async Task<PersonInfo> GetOneAsync(Guid ID)
		{
			var person = (await _personContext.GetAsync(p => p.ID == ID)).FirstOrDefault();
			
			if (person == null)
			{
				throw new ChatAppException($"Person with id {ID} does not exist!");
			}

			var personInfo = new PersonInfo
			{
				ID = person.ID,
				Username = person.Username,
				FirstName = person.FirstName,
				LastName = person.LastName
			};

			return personInfo;
		}

		public async Task<IEnumerable<PersonInfo>> GetManyAsync(IEnumerable<Guid> IDs)
		{
			var persons = await _personContext.GetAsync(p => IDs.Contains(p.ID));

			var personsInfo = persons.Select(p => new PersonInfo
			{
				ID = p.ID,
				Username = p.Username,
				FirstName = p.FirstName,
				LastName = p.LastName
			});

			return personsInfo;
		}
	}
}
