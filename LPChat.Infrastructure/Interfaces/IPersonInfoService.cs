using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LPChat.Common.Models;

namespace LPChat.Infrastructure.Interfaces
{
	public interface IUserService
	{
		Task<IEnumerable<UserModel>> GetManyAsync(IEnumerable<Guid> IDs);
		Task<UserModel> GetOneAsync(Guid personId);
		string GetPersonDisplayName(UserModel personInfo);
	}
}