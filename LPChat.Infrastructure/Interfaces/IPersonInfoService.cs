using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LPChat.Infrastructure.ViewModels;

namespace LPChat.Infrastructure.Interfaces
{
	public interface IPersonInfoService
	{
		Task<IEnumerable<UserModel>> GetManyAsync(IEnumerable<Guid> IDs);
		Task<UserModel> GetOneAsync(Guid personId);
		string GetPersonDisplayName(UserModel personInfo);
	}
}