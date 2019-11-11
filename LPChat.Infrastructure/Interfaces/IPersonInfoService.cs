using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LPChat.Infrastructure.ViewModels;

namespace LPChat.Infrastructure.Interfaces
{
	public interface IPersonInfoService
	{
		Task<IEnumerable<PersonInfoViewModel>> GetManyAsync(IEnumerable<Guid> IDs);
		Task<PersonInfoViewModel> GetOneAsync(Guid personId);
		string GetPersonDisplayName(PersonInfoViewModel personInfo);
	}
}