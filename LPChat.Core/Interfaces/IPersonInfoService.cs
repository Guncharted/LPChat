using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LPChat.Domain.DTO;

namespace LPChat.Domain.Interfaces
{
	public interface IPersonInfoService
	{
		Task<IEnumerable<PersonInfo>> GetManyAsync(IEnumerable<Guid> IDs);
		Task<PersonInfo> GetOneAsync(Guid personId);
		string GetPersonDisplayName(PersonInfo personInfo);
	}
}