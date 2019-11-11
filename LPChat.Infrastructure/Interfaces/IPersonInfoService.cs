using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LPChat.Infrastructure.ViewModels;

namespace LPChat.Infrastructure.Interfaces
{
	public interface IPersonInfoService
	{
		Task<IEnumerable<UserInfoViewModel>> GetManyAsync(IEnumerable<Guid> IDs);
		Task<UserInfoViewModel> GetOneAsync(Guid personId);
		string GetPersonDisplayName(UserInfoViewModel personInfo);
	}
}