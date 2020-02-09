using LPChat.Domain.Entities;
using LPChat.Infrastructure.Interfaces;
using LPChat.Infrastructure.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
	public class PersonInfoServiceTests
	{
		private readonly Mock<IRepositoryManager> repoManager;
		private readonly Mock<IRepository<User>> personRepository;

		private readonly Guid singleUserTestId;

		public PersonInfoServiceTests()
		{
			repoManager = new Mock<IRepositoryManager>();
			personRepository = new Mock<IRepository<User>>();
			singleUserTestId = new Guid();
		}

		[SetUp]
		public void Setup()
		{
			var id = singleUserTestId;
            var list = new List<User> {
                new User
                {
                    ID = id,
                    FirstName = "Oleg",
                    LastName = "Gonchar",
                    Username = "hanchar.aleh@gmail.com",
                    LastUpdatedUtcDate = DateTime.UtcNow
                }
            };
            personRepository.Setup(x => x.FindById(It.IsAny<Guid>()))
                .Returns((Guid pId) => {
                    var result = list.FirstOrDefault(x => x.ID == pId);
                    return Task.FromResult(result);
                });
				

			repoManager.Setup(x => x.GetRepository<User>()).Returns(personRepository.Object);
		}

		[Test]
		public void GetOneUserById()
		{
			var personService = new PersonInfoService(repoManager.Object, null);
			var person = personService.GetOneAsync(singleUserTestId).Result;
			Assert.AreEqual("hanchar.aleh@gmail.com", person.Username);
			Assert.AreEqual("Oleg", person.FirstName);
			Assert.AreEqual("Gonchar", person.LastName);
		}
	}
}